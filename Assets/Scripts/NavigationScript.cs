using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneType{
    Input = 0,
    Output = 2,
    Camera = 1
}
/// <summary>
/// Navigation controller.
/// </summary>
public class NavigationScript : MonoBehaviour
{

    [SerializeField]
    private GameObject UIPanel;
    [SerializeField]
    private GameObject BasePanel;

    private int _currScene;
    // Start is called before the first frame update
    void Start()
    {
        if(Debug.isDebugBuild) {
            SceneManager.LoadSceneAsync("Scenes/PictureScene",LoadSceneMode.Additive);
           _currScene = (int)SceneType.Camera;
           UIPanel.SetActive(true);
           BasePanel.SetActive(false);
        }
        else{
            SceneManager.LoadSceneAsync("Scenes/Demo/TaskListScreen",LoadSceneMode.Additive);
            UIPanel.SetActive(false);
            BasePanel.SetActive(true);
        }
    }

    [SerializeField]
    public void changeScene(int sceneType){
        SceneManager.UnloadSceneAsync(getScenePath(_currScene));
        SceneManager.LoadSceneAsync(getScenePath(sceneType),LoadSceneMode.Additive);
        _currScene = sceneType;
    }

    private string getScenePath(int sceneType){
        string path = "Scenes/";
        switch ((SceneType)sceneType)
        {
            case SceneType.Input:
              return  path += "InputScene";
            case SceneType.Output:
                return path += "OutputScene";
            default:
            case SceneType.Camera:
                 return path += "PictureScene";
        }
    }
}
