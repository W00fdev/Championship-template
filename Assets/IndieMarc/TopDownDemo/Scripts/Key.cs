using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    // Скрипт ключа
    public class Key : MonoBehaviour
    {
        [Header("Индекс ключа для открытия двери")]
        public int key_index = 0;

        [Header("Вес ключа (если нужно несколько для открытия)")]
        public int key_value = 1;

        void Start()
        {
            if (TryGetComponent<CarryItem>(out CarryItem carry_item))
            {
                carry_item.OnTake += OnTake;
                carry_item.OnDrop += OnDrop;
            }    
        }

        private void OnTake(GameObject triggerer)
        {
            
        }

        private void OnDrop(GameObject triggerer)
        {
            
        }

        public bool TryOpenDoor(GameObject door)
        {
            if (door.GetComponent<Door>() && door.GetComponent<Door>().CanKeyUnlock(this) && !door.GetComponent<Door>().IsOpened())
            {
                door.GetComponent<Door>().UnlockWithKey(key_value);
                Destroy(gameObject);
                return true;
            }
            return false;
        }
    }

}