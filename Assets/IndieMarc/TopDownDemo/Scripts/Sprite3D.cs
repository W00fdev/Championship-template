using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    // Компонент для sorting layer имитации в 3Д объектах
    public class Sprite3D : MonoBehaviour
    {
        public string sort_layer = "Default";
        public int sort_order = 0;
        public float z_offset = 0f;
        public bool fix_offset = false;

        void Awake()
        {
            GetComponent<MeshRenderer>().sortingLayerID = SortingLayer.NameToID(sort_layer);
            GetComponent<MeshRenderer>().sortingOrder = sort_order;
            enabled = false;
        }
    }

}