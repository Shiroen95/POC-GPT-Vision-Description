using System.Collections;
using System.Collections.Generic;
using Networking.DTO;
using Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
            _currImage.rectTransform.sizeDelta = DataScript.image.Size();
            _currImage.sprite = Sprite.Create(DataScript.image,new Rect(0, 0, DataScript.image.width/2, DataScript.image.height/2),Vector2.zero);
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
