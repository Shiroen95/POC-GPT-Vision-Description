
using Demo.DataObject;
using Demo.DTO;
using Unity.XR.CoreUtils;
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
        DemoDataScript.Instance.onFinishTask += removeFromTasklist;
    }
    void OnDisable(){
        DemoDataScript.Instance.onAddedNewTask -= addTaskToTasklist;
        DemoDataScript.Instance.onFinishTask -= removeFromTasklist;
    }
    void Start(){
        DemoDataScript.Instance.checkForInternalData();
    }

    private void addTaskToTasklist((int,CleaningTask) taskValue){
        var newTask = Instantiate(taskObject,scrollViewContent.transform);
        newTask.GetComponentInChildren<TaskObjectData>().task = taskValue.Item2;
        newTask.GetComponentInChildren<TaskObjectData>().index = taskValue.Item1;
        newTask.SetActive(true);
    }

    private void removeFromTasklist(int index){
        foreach(Transform child in scrollViewContent.transform){
            if(child.GetComponentInChildren<TaskObjectData>().index == index){
                Destroy(child.gameObject);
                return;
            }
        }
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
