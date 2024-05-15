using Networking.DTO;
using Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Unity;


public class InputScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField systemContent; 
    [SerializeField]
    private TMP_InputField userContent;
    [SerializeField]
    private Image _currImage;
    // Start is called before the first frame update
    void Start()
    {
        var messages = DataScript.request.messages;
        systemContent.text = ((SystemRole)messages[0]).content;

        var userVisionContext = (UserRoleVision)DataScript.request.messages[1];
        userContent.text = ((UserTextContent)userVisionContext.content[0]).text;

        userContent.onSubmit.AddListener((text)=>{
            ((UserTextContent)userVisionContext.content[0]).text = text;
        });
        systemContent.onSubmit.AddListener((text)=>{
            ((SystemRole)messages[0]).content = text;
        });

        if(DataScript.image != null){
            _currImage.rectTransform.sizeDelta = new Vector2(DataScript.image.width, DataScript.image.height);
            _currImage.sprite = Sprite.Create(DataScript.image,new Rect(0, 0, DataScript.image.width, DataScript.image.height),Vector2.zero);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        userContent.onSubmit.RemoveAllListeners();
        systemContent.onSubmit.RemoveAllListeners();
    }
}
