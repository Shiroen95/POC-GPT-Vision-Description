using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Demo.ScreenController{
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

    public void addTask_old(){
        var task = DemoDataScript.Instance.addCleaningTask(new CleaningTask("Test","Test"));
        var newTask = Instantiate(taskObject,scrollViewContent.transform);
        newTask.GetComponent<TaskObjectData>().task = task.Item2;
        newTask.GetComponent<TaskObjectData>().index = task.Item1;
        newTask.SetActive(true);
    }

    public void addTask(){
        DemoDataScript.Instance.currModifyMode = modifyMode.create;
        SceneManager.LoadSceneAsync("Scenes/Demo/ModifyTaskScene",LoadSceneMode.Additive);
    }

    public void showTask(GameObject gameObject){
        DemoDataScript.Instance.currModifyMode = modifyMode.edit;
        Debug.Log(gameObject.GetComponent<TaskObjectData>().index);
    }
}
}
