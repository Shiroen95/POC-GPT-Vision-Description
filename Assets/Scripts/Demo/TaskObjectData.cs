using System;
using TMPro;
using UnityEngine;

public class TaskObjectData : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dataText;
    public CleaningTask task;

    [NonSerialized]
    public int index;

    public void OnEnable(){
        dataText.text = task.Name;
    }
}
