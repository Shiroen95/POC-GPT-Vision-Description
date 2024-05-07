using System.IO;
using UnityEngine;

namespace Config {
    public class ConfigLoader{
        public static APISetting LoadConfig(string path ="./config.json" ) {
        //Get the JSON string from the file on disk.
        string savedJson = File.ReadAllText(path);
        //Convert the JSON string back to a ConfigData object.
        return  JsonUtility.FromJson<APISetting>(savedJson);
        }
    }
}