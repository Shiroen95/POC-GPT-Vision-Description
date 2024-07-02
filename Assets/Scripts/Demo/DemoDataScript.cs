using System.Collections.Generic;

public class DemoDataScript{
    private int index; 
    private Dictionary<int,CleaningTask> taskList;

    private DemoDataScript(){
        createNewDataList();
    }

    private void createNewDataList(){
        taskList = new Dictionary<int,CleaningTask>();
        index = 0;
    }
    private static DemoDataScript _instance = new DemoDataScript();

    public static DemoDataScript Instance {
        get => _instance;
    }

    public (int,CleaningTask) addCleaningTask(CleaningTask cleaningTask){
        taskList.Add(index,cleaningTask);
        var returnValue = (index,cleaningTask);
        index++;
        return returnValue;
    }

    public CleaningTask GetCleaningTask(int index) => taskList.GetValueOrDefault(index);
}