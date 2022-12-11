using UnityEngine;


namespace IndieMarc.TopDown
{
    // Управление персонажем, настройка клавиш
    public class PlayerControls : MonoBehaviour
    {
        public int player_id;
        public KeyCode left_key;
        public KeyCode right_key;
        public KeyCode up_key;
        public KeyCode down_key;
        public KeyCode action_key;

        private Vector2 move = Vector2.zero;
        private bool action_press = false;
        private bool action_hold = false;

        public static PlayerControls Instance;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            move = Vector2.zero;
            action_hold = false;
            action_press = false;

            if (Input.GetKey(left_key))
                move += -Vector2.right;
            if (Input.GetKey(right_key))
                move += Vector2.right;
            if (Input.GetKey(up_key))
                move += Vector2.up;
            if (Input.GetKey(down_key))
                move += -Vector2.up;
            if (Input.GetKey(action_key))
                action_hold = true;
            if (Input.GetKeyDown(action_key))
                action_press = true;

            float move_length = Mathf.Min(move.magnitude, 1f);
            move = move.normalized * move_length;
        }


        //------ These functions should be called from the Update function, not FixedUpdate
        public Vector2 GetMove()
        {
            return move;
        }

        public bool GetActionDown()
        {
            return action_press;
        }

        public bool GetActionHold()
        {
            return action_hold;
        }
    }
}