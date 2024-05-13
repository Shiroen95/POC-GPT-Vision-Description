using System.Collections;
using System.Collections.Generic;
using Networking.DTO;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text systemContent; 
    [SerializeField]
    private TMP_Text userContent;
    [SerializeField]
    private Image currImage;
    // Start is called before the first frame update
    void Start()
    {
        systemContent.text = (string) DataScript.request.messages[0].content;
        var content = (UserContent[])DataScript.request.messages[1].content;
        userContent.text = ((UserTextContent)content[0]).text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
