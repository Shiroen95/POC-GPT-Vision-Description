using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExportScript : MonoBehaviour
{
    private ExportScript(){
        
    }
    private static ExportScript _instance;
    public static ExportScript instance{get
        {
            if(_instance==null){
                _instance = new ExportScript();
            }
            return _instance;
        }
    }

   
}
