using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagController : MonoBehaviour
{

    [SerializeField]
    public TMP_Text tagText;
    [SerializeField]
    Image tagImage;
    [NonSerialized]
    public bool selected = false;
    // Start is called before the first frame update
    public void onSelect(){
        selected = !selected;
        tagImage.color = selected? new Color32(0x2A,0x00,0x91,0xFF) : new Color32(0x49,0x00,0xFF,0xFF);
    }
}
