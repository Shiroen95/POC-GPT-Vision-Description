using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.DataObject;
using Demo.DTO;
using Newtonsoft.Json;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo.ScreenController{
  
    public class ModifyTaskSceneController : MonoBehaviour
    {   
        private enum Screen{
            selection = 0,
            picture = 1,
            userInput = 2,
            sendRequest = 3,
            taskScreen = 4,
        }
        private Dictionary<InteractionMode,Screen[]> interactionRoutes = new Dictionary<InteractionMode, Screen[]>
        {
            {
                InteractionMode.baseMethod, new [] {Screen.taskScreen}
            },
            {
                InteractionMode.semi1Method, new [] {Screen.picture,Screen.userInput,Screen.sendRequest,Screen.taskScreen}
            },
            {
                InteractionMode.semi2Method, new [] {Screen.picture,Screen.sendRequest,Screen.userInput,Screen.sendRequest,Screen.taskScreen}   
            },
            {
                InteractionMode.fullAutoMethod, new [] {Screen.picture,Screen.sendRequest,Screen.sendRequest,Screen.taskScreen}
            }   
        };
        private int currentStep = 0;
        [SerializeField]
        private GameObject _selectionScreen;
        [SerializeField]
        private GameObject _pictureScreen;
        [SerializeField]
        private GameObject _userInputScreen;
        [SerializeField]
        private GameObject _taskScreen;
        [SerializeField]
        private GameObject _sendPanel;

        [SerializeField]
        private TaskScreenObjectData _taskScreenObjectData;
        [SerializeField]
        private PictureObjectData _pictureObjectData;
        [SerializeField]
        private UserInputObjectData _userInputObjectData;
        [SerializeField]
        private SendingController _sendingController;
        private CleaningTask currentTask = null;
        private List<string> selectedTags = new List<string>();
        void Start()
        {
            DemoDataScript.Instance.currentInteractionMode = InteractionMode.baseMethod;
            currentStep = 0;
            _selectionScreen.SetActive(false);
            _pictureScreen.SetActive(false);
            _userInputScreen.SetActive(false);
            _taskScreen.SetActive(false);

            switch (DemoDataScript.Instance.currModifyMode){
                case ModifyMode.edit:
                    currentTask = DemoDataScript.Instance.currCleaningTask;
                    _taskScreen.SetActive(true);
                    break;
                case ModifyMode.create:
                    currentTask = new CleaningTask();
                    _selectionScreen.SetActive(true);
                    break;
                case ModifyMode.none:
                    SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
                    break;
            }
            fillTaskScreen();
        }
        public void saveData(){
            if(_taskScreenObjectData.headlineIf.text !=""){
                currentTask.Name = _taskScreenObjectData.headlineIf.text;
                currentTask.Description = _taskScreenObjectData.descriptionIf.text;

                if(DemoDataScript.Instance.currModifyMode == ModifyMode.create)
                    DemoDataScript.Instance.addCleaningTask(currentTask);
            }
            DemoDataScript.Instance.currModifyMode = ModifyMode.none;
            SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
        }
        private void fillTaskScreen(){
            _taskScreenObjectData.headlineIf.text = currentTask.Name;
            _taskScreenObjectData.descriptionIf.text = currentTask.Description;
        }
        public async void nextStepAsync(){
            try{
                var nextStep = interactionRoutes[DemoDataScript.Instance.currentInteractionMode][currentStep+1];
                currentStep++;
                if(nextStep == Screen.sendRequest){
                     await requestStepAsync();
                }
                else{
                openScreen(nextStep);
                }
                
            }
            catch(IndexOutOfRangeException){
                Debug.Log("outofRange");
                currentStep --;
            }
        }
        private async Task requestStepAsync(){
            _sendPanel.SetActive(true);
            var jsonContent = await _sendingController.requestStep();
            setData(DemoDataScript.Instance.currentRequestMode,jsonContent);
            _sendPanel.SetActive(false);
            nextStepAsync();   
        }
        public void setIfAnnotations(){
            DemoDataScript.Instance.annotationList.annotation = _userInputObjectData.tagsIf.text;
        }
        public void setInteractionMode(int mode){
           DemoDataScript.Instance.currentInteractionMode = (InteractionMode)mode;
           openScreen(interactionRoutes[DemoDataScript.Instance.currentInteractionMode][0]);
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
                    fillUserInputScreen();
                    _userInputScreen.SetActive(true);
                    break;
                case Screen.taskScreen:
                    fillTaskScreen();
                    _taskScreen.SetActive(true);
                    break;

            }
        } 
        private void fillUserInputScreen(){
            if(DemoDataScript.Instance.currentInteractionMode == InteractionMode.semi1Method){
            _userInputObjectData.tagsIf.gameObject.SetActive(true);
            _userInputObjectData.tagList.SetActive(false);
           }
           else if(DemoDataScript.Instance.currentInteractionMode == InteractionMode.semi2Method){
            generateTagList();
            _userInputObjectData.tagsIf.gameObject.SetActive(false);
            _userInputObjectData.tagList.SetActive(true);
           }  
        }

        private void generateTagList(){
            var annotationStringList = DemoDataScript.Instance.annotationList.annotation.Split(",");
            foreach(var annotation in annotationStringList){
                if(annotation != ""){
                var tag = Instantiate(_userInputObjectData.tagTemplate, _userInputObjectData.tagTemplate.transform.parent);
                tag.GetComponentInChildren<TMP_Text>().text = annotation;
                tag.SetActive(true);
                }
            }
            DemoDataScript.Instance.annotationList.annotation="";
        }

        public void selectPicture(){
            PictureService.PickImage(524, setPicture);
        }
        public void takePicture(){
            PictureService.TakePicture(524, setPicture);
        }
        private void setPicture(Texture2D input){
            DemoDataScript.Instance.currImage = input;
            Debug.Log(_pictureObjectData.image);
            _pictureObjectData.image.rectTransform.sizeDelta = 
                new Vector2(DemoDataScript.Instance.currImage.width, DemoDataScript.Instance.currImage.height);
            _pictureObjectData.image.sprite = Sprite.Create(
                DemoDataScript.Instance.currImage,
                new Rect(0, 0, DemoDataScript.Instance.currImage.width, DemoDataScript.Instance.currImage.height),Vector2.zero);
            _pictureObjectData.image.enabled = true; 
            _userInputObjectData.image = _pictureObjectData.image;
        }
        private void setData(RequestMode requestMode, string jsonContent){
            switch(requestMode){
                case RequestMode.userTags:
                case RequestMode.autoTags:
                    DemoDataScript.Instance.annotationList 
                        = JsonConvert.DeserializeObject<AnnotationList>(jsonContent);
                    break;
                case RequestMode.task:
                    currentTask = JsonConvert.DeserializeObject<CleaningTask>(jsonContent);
                    break;
            }
        }
        public void onTagClicked(GameObject tag){
            var tagController = tag.GetComponent<TagController>();
            if(!tagController.selected)
            {
                selectedTags.Add(tagController.tagText.text);
            }
            else{
                selectedTags.Remove(tagController.tagText.text);
            }
            tagController.onSelect();
            selectedTags.ForEach(x => Debug.Log(x));
        }
    }
}