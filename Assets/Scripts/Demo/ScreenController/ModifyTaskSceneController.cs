using System;
using System.Collections.Generic;
using Demo.DataObject;
using Demo.DTO;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo.ScreenController{
  
    public class ModifyTaskSceneController : MonoBehaviour
    {   
        private enum Screen{
            selection = 0,
            picture = 1,
            userInput = 2,
            taskScreen = 3,
        }
        private enum InteractionMode {
            baseMethod = 0,
            semi1Method = 1,
            semi2Method = 2,
            fullAutoMethod = 3,
        }
        private Dictionary<InteractionMode,Screen[]> interactionRoutes = new Dictionary<InteractionMode, Screen[]>
        {
            {
                InteractionMode.baseMethod, new [] {Screen.taskScreen}
            },
            {
                InteractionMode.semi1Method, new [] {Screen.picture,Screen.userInput,Screen.taskScreen}
            },
            {
                InteractionMode.semi1Method, new [] {Screen.picture,Screen.userInput,Screen.taskScreen}   
            },
            {
                InteractionMode.fullAutoMethod, new [] {Screen.picture,Screen.taskScreen}
            }   
        };
         
        private int currentStep = 0;
        private InteractionMode currentInteractionMode = InteractionMode.baseMethod;
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
            currentInteractionMode = InteractionMode.baseMethod;
            currentStep = 0;
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
                    currentTask = new CleaningTask();
                    _selectionScreen.SetActive(true);
                    break;
                case modifyMode.none:
                    SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
                    break;
            }
            fillTaskScreen();
        }
        public void saveData(){
            if(_taskScreenObjectData.headlineIf.text.IsNotNullOrEmpty()){
                currentTask.Name = _taskScreenObjectData.headlineIf.text;
                currentTask.Description = _taskScreenObjectData.descriptionIf.text;

                if(DemoDataScript.Instance.currModifyMode == modifyMode.create)
                    DemoDataScript.Instance.addCleaningTask(currentTask);
            }
            DemoDataScript.Instance.currModifyMode = modifyMode.none;
            SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
        }
        public void fillTaskScreen(){
            _taskScreenObjectData.headlineIf.text = currentTask.Name;
            _taskScreenObjectData.descriptionIf.text = currentTask.Description;
        }

        [SerializeField]
        private void nextStep(){
            try{
                openScreen(interactionRoutes[currentInteractionMode][currentStep++]);
            }
            catch(IndexOutOfRangeException){
                currentStep--;
            }
        }
        [SerializeField]
        private void setInteractionMode(InteractionMode mode){
           currentInteractionMode = mode;
           openScreen(0);
        }


        private void openScreen(Screen screen){
            _selectionScreen.SetActive(false);
            _pictureScreen.SetActive(false);
            _userInputScreen.SetActive(false);
            _taskScreen.SetActive(false);

            switch(screen){
                case Screen.selection:
                    _selectionScreen.SetActive(true);
                    break;
                case Screen.picture:
                    _pictureScreen.SetActive(true);
                    break;
                case Screen.userInput:
                    _userInputScreen.SetActive(true);
                    break;
                case Screen.taskScreen:
                    _taskScreen.SetActive(true);
                    break;

            }
        }
    }
}