using System;
using System.IO;
using UnityEngine;

namespace Config {
    /// <summary>
    /// Config loader for APISettings. Config file needs to be present in (Assets/Ressources/Config/config.json)
    /// </summary>
    public class ConfigLoader{
        public static APISetting LoadConfig(string path = "Config/config" ) {
        //Get the JSON string from the file on disk.
        var savedJson =  Resources.Load<TextAsset>(path);;
        //Convert the JSON string back to a ConfigData object.
        return  JsonUtility.FromJson<APISetting>(savedJson.text);
        }
    }
}