using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IndieMarc.TopDown
{
    // Скрипт открытия двери
    public class Door : MonoBehaviour
    {
        [Header("Кол-во рычагов для открытия")]
        public int nb_switches_required = 1;

        [Header("Открытие в другую сторону")]
        public bool reversed_side = false;

        [Header("Открыт со старта")]
        public bool opened_at_start = false;

        [Header("Возвращение в норм. состояние при смерти игрока")]
        public bool reset_on_death = true;

        [Header("Скорость открытия")]
        public float open_speed;

        [Header("Скорость закрытия")]
        public float close_speed;

        [Header("Величина смещения")]
        public float max_move;

        [Space]
        [Header("Можно ли открыть ключом?")]
        public bool key_can_open;

        [Header("Индекс ключа для открытия")]
        public int key_index;

        [Space]
        [Header("Состояние рычага для открытия")]
        public LeverState lever_state_required;

        [Header("Массив рычагов")]
        public GameObject[] levers;

        [Header("Звуки при открытии/закрытии")]
        public AudioClip audio_door_open;
        public AudioClip audio_door_close;
        public AudioClip audio_door_close_hard;
        
        private Vector3 initialPos;
        private int nb_keys_inside;
        private int audio_last_played;
        private bool initial_opened;
        private Vector3 target_pos;
        private bool should_open;

        private AudioSource audio_source;
        
        private List<Lever> lever_list = new List<Lever>();

        private static List<Door> door_list = new List<Door>();

        void Awake()
        {
            door_list.Add(this);
        }

        void OnDestroy()
        {
            door_list.Remove(this);
        }

        void Start()
        {
            initialPos = transform.position;
            initialPos.z = 0f;
            initial_opened = opened_at_start;
            audio_source = GetComponent<AudioSource>();
            target_pos = transform.position;
            should_open = opened_at_start;

            foreach (GameObject lever in levers)
                InitSwitch(lever);
            
            ResetOne();
        }

        private void SetOpenedInstant()
        {
            Vector3 move_dir = GetMoveDir();
            transform.position = initialPos + move_dir * max_move;
            target_pos = transform.position;
        }

        private void InitSwitch(GameObject swt)
        {
            if (swt != null)
            {
                if (swt.GetComponent<Lever>())
                    lever_list.Add(swt.GetComponent<Lever>());
            }
        }

        void FixedUpdate()
        {
            //Get nb switch on
            int nb_switch = GetNbSwitches();

            //keys
            nb_switch += nb_keys_inside;

            //Open door
            bool activated = (nb_switch >= nb_switches_required);
            Vector3 move_dir = GetMoveDir();
            should_open = opened_at_start ? !activated : activated;
            target_pos = transform.position;
            
            if (should_open)
            {
                Vector3 diff = transform.position - initialPos;
                if (open_speed >= 0.01f && diff.magnitude < max_move)
                {
                    target_pos = initialPos + move_dir.normalized * max_move;
                    target_pos.z = 0f;

                    if (audio_source.enabled && !audio_source.isPlaying && audio_last_played != 1)
                    {
                        audio_last_played = 1;
                        audio_source.clip = audio_door_open;
                        audio_source.Play();
                    }
                }
            }
            else
            {
                Vector3 diff = transform.position - initialPos;
                float dot_prod = Vector3.Dot(diff, move_dir);

                if (close_speed >= 0.01f && dot_prod > 0.001f && diff.magnitude > 0.01f)
                {
                    target_pos = initialPos;
                    target_pos.z = 0f;

                    if (audio_source.enabled && !audio_source.isPlaying && audio_last_played != 2)
                    {
                        audio_last_played = 2;
                        audio_source.clip = close_speed > 5.1f ? audio_door_close_hard : audio_door_close;
                        audio_source.Play();
                    }
                }
            }
        }

        private void Update()
        {
            Vector3 move_dir = target_pos - transform.position;
            if (move_dir.magnitude > 0.01f)
            {
                float speed = should_open ? open_speed : close_speed;
                float move_dist = Mathf.Min(speed * Time.deltaTime, move_dir.magnitude);
                transform.position += move_dir.normalized * move_dist;
            }
        }

        private Vector3 GetMoveDir()
        {
            float door_dir = reversed_side ? -1f : 1f;
            Vector3 ori_dir = Vector3.right;
            Vector3 move_dir = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * ori_dir * door_dir;
            move_dir.z = 0f;
            return move_dir.normalized;
        }

        public bool IsOpened()
        {
            float dist = (transform.position - initialPos).magnitude;
            return dist > (max_move / 2f);
        }

        private int GetNbSwitches()
        {
            int nb_switch = 0;
            
            // Lever ------------
            foreach (Lever lever in lever_list)
            {
                if (lever)
                    nb_switch += (lever.state == lever_state_required) ? lever.door_value : 0;
            }
            
            return nb_switch;
        }

        public void Open()
        {
            opened_at_start = true;
        }

        public void Close()
        {
            opened_at_start = false;
        }

        public void Toggle()
        {
            opened_at_start = !opened_at_start;
        }

        public bool CanKeyUnlock(Key key)
        {
            return (key_can_open && key.key_index == key_index);
        }

        public void UnlockWithKey(int value)
        {
            nb_keys_inside += value;
        }

        public void ResetOne()
        {
            opened_at_start = initial_opened;
            if (opened_at_start)
                SetOpenedInstant();
            else
                transform.position = initialPos;
            target_pos = transform.position;
            should_open = opened_at_start;
        }

        public static void ResetAll()
        {
            foreach (Door door in door_list)
            {
                if (door.reset_on_death)
                    door.ResetOne();
            }
        }
    }

}