using UnityEngine;
using UnityEngine.SceneManagement;

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
    void Start()
    {
        _selectionScreen.SetActive(false);
        _pictureScreen.SetActive(false);
        _userInputScreen.SetActive(false);
        _taskScreen.SetActive(false);

        switch (DemoDataScript.Instance.currModifyMode){
            case modifyMode.edit:
                _taskScreen.SetActive(true);
                break;
             case modifyMode.create:
                _selectionScreen.SetActive(true);
                break;
            case modifyMode.none:
                SceneManager.UnloadSceneAsync("Scenes/Demo/ModifyTaskScene");
                break;
        }
    }
}
