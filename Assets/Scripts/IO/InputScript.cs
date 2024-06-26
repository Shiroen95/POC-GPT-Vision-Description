using System;
using Networking.DTO;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using  IO;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
/// Input handler script.
/// </summary>
public class InputScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField systemContent; 
    [SerializeField]
    private TMP_InputField userContent;
    [SerializeField]
    private Image _currImage;
    [SerializeField]
    private TMP_Dropdown _dropDown;
    [SerializeField]
    private Toggle _detailToggle;
    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private GameObject sendingPanel;
    [SerializeField]
    private TMP_InputField BatchInput;

    private int _maxImgSize = 524;

 
    // Start is called before the first frame update
    void Start()
    {
        ImportScript.onImportSettings += SetupInputFields;
        SetupInputFields(); 
    }

    private async Task sendRequest (){
        if(DataScript.image == null){
            Debug.Log("No Image found");
            return;
        }
        await RESTClient.instance.sendGPT4PostRequest(Convert.ToBase64String(DataScript.image?.EncodeToJPG()));
    }

    public async void sendBatchRequest(){
        sendingPanel.SetActive(true);
        var Tasklist = new List<Task>();
        int.TryParse(BatchInput.text,out int result);
        if(result > 1 ){
            for (int i = 0; i < result; i++)
            {
                Tasklist.Add(sendRequest());
            }
            await Task.WhenAll(Tasklist);
        }
        else{
            await sendRequest();
        }
        
        sendingPanel.SetActive(false);
    }

    private void SetupInputFields(){
        var messages = DataScript.request.messages;
        systemContent.text = ((SystemRole)messages[0]).content;

        var userVisionContext = (UserRoleVision)DataScript.request.messages[1];
        userContent.text = ((UserTextContent)userVisionContext.content[0]).text;
        _detailToggle.isOn = ((UserVisionContent)userVisionContext.content[1]).image_url.detail == "high";
        
        _detailToggle.onValueChanged.AddListener((value)=>{
            ((UserVisionContent)userVisionContext.content[1]).image_url.detail = value ? "high":"low"; 
        });
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
    
    void OnDestroy()
    {
        _detailToggle.onValueChanged.RemoveAllListeners();
        userContent.onSubmit.RemoveAllListeners();
        systemContent.onSubmit.RemoveAllListeners();
        ImportScript.onImportSettings -= SetupInputFields;
    }
    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
        {
            Debug.Log( "Image path: " + path );
            if( path != null )
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, _maxImgSize ,false);
                if( texture == null )
                {
                    Debug.Log( "Couldn't load texture from " + path );
                    return;
                }
                Debug.Log("height: " + texture.height);
                Debug.Log("width: " + texture.width);
                DataScript.image = texture;
                SetupInputFields();
            }
        } );
        
        Debug.Log( "Permission result: " + permission );
    }

    public void OnDropdownValueChanged(){
        int.TryParse(_dropDown.options[_dropDown.value].text, out _maxImgSize);
    }

    public void togglePannel(){
       _panel.SetActive(!_panel.activeSelf);
    }


}
