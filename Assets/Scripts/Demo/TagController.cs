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
        tagImage.color = selected? Color.blue : Color.green;
    }
}
