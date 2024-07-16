using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.DTO;
using Networking.DTO;
using Newtonsoft.Json;
using UnityEngine;

namespace Demo{

    public enum ModifyMode{
        edit,
        create, 
        none
    }
    public enum RequestMode{
        userTags,
        autoTags,
        task
    }
    public enum InteractionMode {
            baseMethod = 0,
            semi1Method = 1,
            semi2Method = 2,
            fullAutoMethod = 3,
    }

    public class DemoDataScript{

        private BaseImageDTO _request  = new BaseImageDTO(){
            max_tokens = 1000,
            messages = new List<GPTRoles>{
                new SystemRole(){
                    content =""},
                new UserRoleVision(){
                        content = new UserContent[]{
                            new UserTextContent(){
                                text = ""
                            },
                            new UserVisionContent(){
                                image_url = new(){
                                    url ="data:image/jpeg;base64,",
                                    detail ="low"
                                }
                            },
                        }
                    }},
        };
        public AnnotationList annotationList = new AnnotationList();
        public BaseImageDTO request {
            get {
                setContent();
                return _request;
            }
        }
        public InteractionMode _currentInteractionMode = InteractionMode.baseMethod;
        public InteractionMode currentInteractionMode {
            get => _currentInteractionMode;
            set{
                annotationList = new AnnotationList();
                _currentInteractionMode = value;
            }
        }

        public RequestMode currentRequestMode = RequestMode.task;
        public Texture2D currImage;
        public ModifyMode currModifyMode = ModifyMode.none;
        private int index;
        private Dictionary<int,CleaningTask> taskList;
        private int _currCleaningTaskIndex = -1; 
        private static DemoDataScript _instance = new DemoDataScript();
        public static DemoDataScript Instance {
            get => _instance;
        }
        public CleaningTask currCleaningTask {
            get => _currCleaningTaskIndex ==-1 ? null : GetCleaningTask(_currCleaningTaskIndex);
        }
        public delegate void OnAddedNewTask((int,CleaningTask) taskValue);
        public event OnAddedNewTask onAddedNewTask;
        public delegate void OnFinishTask(int index);
        public event OnFinishTask onFinishTask;

        private string taskTextContent = "";

        private string autoTagsTextContent =    @"The picture shows a cleaning task that has to be done. There is one primary cleaning task with its corresponding object. Only use at max two most relevant object. The primary cleaning task can have sub tasks.
                                                The object dominates the picture. Classify the object and the task and write annotaions for it. Only use at max most relevant object.
                                                Try to use only use one word annotations. Return the annotaions in the following json format:
                                                {""annotation"": ,}
                                                Don't use a json list. Delimit the annotaions with a comma.
                                                Return only the pure json, without any commenting syntax.";

        private string userTagsTextContent =    @"The picture shows a cleaning task that has to be done. There is one primary cleaning task with its corresponding object. The primary cleaning task can have sub tasks.
                                                Classify possible objects and the task and write annotaions for it.
                                                Try to use only use one word annotations. Return the annotaions in the following json format:
                                                {""annotation"": ,}
                                                Don't use a json list. Delimit the annotaions with a comma.
                                                Return only the pure json, without any commenting syntax.";
        private DemoDataScript(){
            createNewDataList();
        }

        private void createNewDataList(){
            taskList = new Dictionary<int,CleaningTask>();
            index = 0;
        }
        private void createFromSave(Dictionary<int,CleaningTask> taskList){
            this.taskList = taskList;
            foreach (var item in taskList)
            {
                onAddedNewTask?.Invoke((item.Key,item.Value));
            }
            index = taskList.Count;
        }
        public async Task checkForInternalData(){
            var data = await SaveFileController.LoadSaveFromDevice();
            if(data != null)
                createFromSave(data);
        }
        public void setCurrentCleaningTask(int index){
            if(taskList.ContainsKey(index))
                _currCleaningTaskIndex = index;
        }
        public (int,CleaningTask) addCleaningTask(CleaningTask cleaningTask){
            taskList.Add(index, cleaningTask);
            var returnValue = (index, cleaningTask);
            index++;
            onAddedNewTask?.Invoke(returnValue);
            SaveFileController.SaveFileToDevice(taskList);
            return returnValue;
        }
        public void finishCurrentCleaningTask(){
            taskList.Remove(_currCleaningTaskIndex);
            onFinishTask?.Invoke(_currCleaningTaskIndex);
            SaveFileController.SaveFileToDevice(taskList);
            _currCleaningTaskIndex=-1;
        }

        public (int,CleaningTask) modifyCleaningTask(CleaningTask cleaningTask, int index){
            if(taskList.ContainsKey(index))
                taskList[index] = cleaningTask;

            return(index, cleaningTask);
        }

        public CleaningTask GetCleaningTask(int index) => taskList.GetValueOrDefault(index);

        private void setAnnotations(){
            taskTextContent = 
                @"Please create a delegation for a cleaning task as described:
                The task should always have a primary goal, which may have sub-tasks that need to be described.
                All responses should be in the following json format:
                {headline:, body:}
                Only return the pure json, without any commenting syntax.
                The content of the json is described as: 
                headline: Describes the main task as briefly as possible.
                body: Give instructions for the main task and the sub-task at hand. If possible, explain how to perform these tasks.
                The Userinput will always be a picture.
                Metatags for the picture are as follows:"+
                annotationList.annotation +
                "The picture will show you the concrete task and it's current state.";
        }
        private void setContent(){
            var role = (UserRoleVision)_request.messages[1];
            var userContent = (UserTextContent)role.content[0];
            if(currentRequestMode == RequestMode.task){
                setAnnotations();
                userContent.text = taskTextContent;
            }
            else if(currentRequestMode == RequestMode.autoTags){
                userContent.text = autoTagsTextContent;
            }
            else{
                userContent.text = userTagsTextContent;
            }
        }
    }
}