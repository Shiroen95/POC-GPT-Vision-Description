using System;
using Networking.DTO;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject _panel;

    private int _maxImgSize = 524;
    // Start is called before the first frame update
    void Start()
    {
        SetupInputFields(); 
    }

    private void SetupInputFields(){
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
    void OnDestroy()
    {
        userContent.onSubmit.RemoveAllListeners();
        systemContent.onSubmit.RemoveAllListeners();
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
