using System;
using Demo.DTO;
using TMPro;
using UnityEngine;

namespace Demo.DataObject{
    public class TaskObjectData : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text dataText;
        public CleaningTask task;

        [NonSerialized]
        public int index = -1;

        public void OnEnable(){
            dataText.text = task.Name;
        }
    }
}
