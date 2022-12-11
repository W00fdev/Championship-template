﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    // Скрипт держания предмета в руке
    [RequireComponent(typeof(PlayerCharacter))]
    public class CharacterHoldItem : MonoBehaviour
    {
        [Header("Объект руки персонажа")]
        public Transform hand;

        private PlayerCharacter character;

        private CarryItem held_item = null;
        private float take_item_timer = 0f;

        void Awake()
        {
            character = GetComponent<PlayerCharacter>();
        }

        private void Start()
        {
            character.onDeath += DropItem;
        }

        void Update()
        {
            PlayerControls controls = PlayerControls.Instance;
            
            take_item_timer += Time.deltaTime;
            if (held_item && controls.GetActionDown())
                held_item.UseItem();
        }

        private void LateUpdate()
        {
            if (held_item != null)
                held_item.UpdateCarryItem();
        }

        public void TakeItem(CarryItem item) {

            if (item == held_item || take_item_timer < 0f)
                return;

            if (held_item != null)
                DropItem();

            held_item = item;
            take_item_timer = -0.2f;
            item.Take(this);
        }

        public void DropItem()
        {
            if (held_item != null)
            {
                held_item.Drop();
                held_item = null;
            }
        }

        public PlayerCharacter GetCharacter()
        {
            return character;
        }

        public CarryItem GetHeldItem()
        {
            return held_item;
        }

        public Vector3 GetHandPos()
        {
            if (hand)
                return hand.transform.position;
            return transform.position;
        }

        public Vector2 GetMove()
        {
            return character.GetMove();
        }

        public Vector2 GetFacing()
        {
            return character.GetFacing();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<CarryItem>())
                TakeItem(collision.GetComponent<CarryItem>());
        }

        void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<Door>() && held_item && held_item.GetComponent<Key>())
            {
                held_item.GetComponent<Key>().TryOpenDoor(coll.gameObject);
            }
        }
    }

}
