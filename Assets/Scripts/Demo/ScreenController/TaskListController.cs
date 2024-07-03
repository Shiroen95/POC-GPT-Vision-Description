
using Demo.DataObject;
using Demo.DTO;
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

    void OnEnable(){
        DemoDataScript.Instance.onAddedNewTask += addTaskToTasklist;
    }
    void OnDisable(){
        DemoDataScript.Instance.onAddedNewTask -= addTaskToTasklist;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void addTaskToTasklist((int,CleaningTask) taskValue){
        var newTask = Instantiate(taskObject,scrollViewContent.transform);
        newTask.GetComponent<TaskObjectData>().task = taskValue.Item2;
        newTask.GetComponent<TaskObjectData>().index = taskValue.Item1;
        newTask.SetActive(true);
    }


    public void addTask(){
        DemoDataScript.Instance.currModifyMode = ModifyMode.create;
        SceneManager.LoadSceneAsync("Scenes/Demo/ModifyTaskScene",LoadSceneMode.Additive);
    }

    public void showTask(GameObject gameObject){
        DemoDataScript.Instance.currModifyMode = ModifyMode.edit;
        DemoDataScript.Instance.setCurrentCleaningTask(gameObject.GetComponent<TaskObjectData>().index);
        SceneManager.LoadSceneAsync("Scenes/Demo/ModifyTaskScene",LoadSceneMode.Additive);
    }
}
}
