using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListController : MonoBehaviour
{
    [SerializeField]
    private Button addTaskBtn;

    [SerializeField]
    private GameObject taskObject;
    [SerializeField]
    private GameObject scrollViewContent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTask(){
        var newTask = GameObject.Instantiate(taskObject);
        newTask.SetActive(true);
        //newTask.transform.parent =  scrollViewContent.transform;
    }

    public void showTask(GameObject gameObject){

    }
}
