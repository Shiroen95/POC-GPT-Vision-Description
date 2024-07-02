using System.Collections.Generic;

public class DemoDataScript{

    public List<CleaningTask> taskList;

    private DemoDataScript(){
        taskList = new List<CleaningTask>();
    }
    private static DemoDataScript _instance = new DemoDataScript();

    public static DemoDataScript Instance {
        get => _instance;
    }
}