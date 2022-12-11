using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.Platformer
{
    // Управление персонажем, настройка клавиш
    public class PlayerControls : MonoBehaviour
    {
        public KeyCode left_key;
        public KeyCode right_key;
        public KeyCode up_key;
        public KeyCode down_key;
        public KeyCode jump_key;
        public KeyCode action_key;

        [Header("Кнопка стрельбы")]
        public KeyCode fire_key;

        private Vector2 move = Vector2.zero;
        private bool jump_press = false;
        private bool jump_hold = false;
        private bool action_press = false;
        private bool action_hold = false;
        private bool fire_down = false;

        public static PlayerControls Instance;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {

            move = Vector2.zero;
            jump_hold = false;
            jump_press = false;
            action_hold = false;
            action_press = false;
            fire_down = false;

            if (Input.GetKey(left_key))
                move += -Vector2.right;
            if (Input.GetKey(right_key))
                move += Vector2.right;
            if (Input.GetKey(up_key))
                move += Vector2.up;
            if (Input.GetKey(down_key))
                move += -Vector2.up;
            if (Input.GetKey(jump_key))
                jump_hold = true;
            if (Input.GetKeyDown(jump_key))
                jump_press = true;
            if (Input.GetKey(action_key))
                action_hold = true;
            if (Input.GetKeyDown(action_key))
                action_press = true;
            if (Input.GetKeyDown(fire_key))
                fire_down = true;

            float move_length = Mathf.Min(move.magnitude, 1f);
            move = move.normalized * move_length;
        }


        //------ These functions should be called from the Update function, not FixedUpdate
        public Vector2 GetMove()
        {
            return move;
        }

        public bool GetJumpDown()
        {
            return jump_press;
        }

        public bool GetJumpHold()
        {
            return jump_hold;
        }

        public bool GetActionDown()
        {
            return action_press;
        }

        public bool GetActionHold()
        {
            return action_hold;
        }

        public bool GetFireDown()
        {
            return fire_down;
        }
    }

}