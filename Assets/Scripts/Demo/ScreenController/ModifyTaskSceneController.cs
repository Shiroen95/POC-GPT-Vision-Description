using Demo.DataObject;
using Demo.DTO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo.ScreenController{
    public enum createStep{
        selection,
        pic
    }
    public class ModifyTaskSceneController : MonoBehaviour
    {   
        [SerializeField]
        private GameObject _selectionScreen;
        [SerializeField]
        private GameObject _pictureScreen;
        [SerializeField]
        private GameObject _userInputScreen;
        [SerializeField]
        private GameObject _taskScreen;

        [SerializeField]
        private TaskScreenObjectData _taskScreenObjectData;

        private CleaningTask currentTask = null;
        void Start()
        {
            _selectionScreen.SetActive(false);
            _pictureScreen.SetActive(false);
            _userInputScreen.SetActive(false);
            _taskScreen.SetActive(false);

            switch (DemoDataScript.Instance.currModifyMode){
                case modifyMode.edit:
                    currentTask = DemoDataScript.Instance.currCleaningTask;
                    _taskScreen.SetActive(true);
                    break;
                case modifyMode.create:
                    //_selectionScreen.SetActive(true);
                    currentTask = new CleaningTask();
                    _taskScreen.SetActive(true);
                    break;
                case modifyMode.none:
                    SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
                    break;
            }
            fillTaskScreen();
        }
        public void saveData(){
            if(DemoDataScript.Instance.currModifyMode == modifyMode.create)
                DemoDataScript.Instance.addCleaningTask(currentTask);
            SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
        }

        public void fillTaskScreen(){
            _taskScreenObjectData.headlineIf.text = currentTask.Name;
            _taskScreenObjectData.descriptionIf.text = currentTask.Description;
        }
    }
}