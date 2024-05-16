using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class ExportScript {
    private string baseDataPath;
    private ExportScript(){
        baseDataPath = Application.persistentDataPath + "/export";
        Directory.CreateDirectory(baseDataPath);
    }
    private static ExportScript _instance;
    public static ExportScript instance{
        get
        {
            if(_instance==null){
                _instance = new ExportScript();
            }
            return _instance;
        }
    }

    public  void saveData (){
        var requestJson = JsonConvert.SerializeObject(DataScript.request);
        var responseJson = JsonConvert.SerializeObject(DataScript.response);
        var jpgImageByteArray = DataScript.image.EncodeToJPG();
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="objectToWrite"></param>
    /// <param name="append"></param>
    public async void WriteBinaryToFile(string filePath, byte[] objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            await stream.WriteAsync(objectToWrite);  
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="objectToWrite"></param>
    /// <param name="append"></param>
    public async void WriteStringToFile(string filePath, string objectToWrite, bool append = false)
    {
        using (StreamWriter streamWriter = new StreamWriter( File.Open(filePath, append ? FileMode.Append : FileMode.Create)))
        {
            await streamWriter.WriteAsync(objectToWrite);  
        }
    }
   
}
