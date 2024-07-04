using TMPro;
using UnityEngine;

namespace Demo.DataObject{
    public class TaskScreenObjectData : MonoBehaviour
    {
        [SerializeField]
        public TMP_InputField headlineIf;
        [SerializeField]
        public TMP_InputField descriptionIf;
        [SerializeField]
        public GameObject finishBtn;
        [SerializeField]
        public GameObject backBtn;
    }
}
