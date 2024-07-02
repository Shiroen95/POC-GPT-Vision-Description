using System.Collections.Generic;

namespace Demo{

    public enum modifyMode{
        edit,
        create, 
        none
    }

    public class DemoDataScript{
        public modifyMode currModifyMode = modifyMode.none;
        private int index;
        private Dictionary<int,CleaningTask> taskList;
        private int _currCleaningTaskIndex = -1; 
        private static DemoDataScript _instance = new DemoDataScript();
        public static DemoDataScript Instance {
            get => _instance;
        }
        public CleaningTask currCleaningTask {
            get => _currCleaningTaskIndex==-1 ? null : GetCleaningTask(_currCleaningTaskIndex);
        }

        private DemoDataScript(){
            var loadedData = checkForInternalData();
            if(loadedData != null)
            {
                taskList = loadedData;
                index = loadedData.Count;
            }
            else
                createNewDataList();
        }

        private void createNewDataList(){
            taskList = new Dictionary<int,CleaningTask>();
            index = 0;
        }

        private Dictionary<int,CleaningTask> checkForInternalData(){
            return null;
        }
        public void setCurrentCleaningTask(int index){
            if(taskList.ContainsKey(index))
                _currCleaningTaskIndex = index;
        }
        public (int,CleaningTask) addCleaningTask(CleaningTask cleaningTask){
            taskList.Add(index, cleaningTask);
            var returnValue = (index, cleaningTask);
            index++;
            return returnValue;
        }

        public (int,CleaningTask) modifyCleaningTask(CleaningTask cleaningTask, int index){
            if(taskList.ContainsKey(index))
                taskList[index] = cleaningTask;

            return(index, cleaningTask);
        }

        public CleaningTask GetCleaningTask(int index) => taskList.GetValueOrDefault(index);
        
    }
}