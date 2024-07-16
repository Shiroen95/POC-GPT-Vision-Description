using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Demo.DTO;
using Newtonsoft.Json;
using UnityEngine;
namespace Demo{
    public class SaveFileController{

            private static string savePath = Application.persistentDataPath + "/saveFile";
        
            /// <summary>
            /// Writes a binary array to a file.
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="objectToWrite"></param>
            /// <param name="append"></param>
            public static async void SaveFileToDevice(Dictionary<int,CleaningTask> cleaningTasks)
            {
                Directory.CreateDirectory(savePath);
                var jsonData = JsonConvert.SerializeObject(cleaningTasks);
                using (StreamWriter stream = new StreamWriter(File.Open(savePath+"/taskList.json", FileMode.Create)))
                {
                    await stream.WriteAsync(jsonData);  
                }
            }

            public static async Task<Dictionary<int,CleaningTask>> LoadSaveFromDevice()
            {       
                    if(!Directory.Exists(savePath))
                        return null;
                    var jsonData = "";
                    using (StreamReader r = new StreamReader(File.Open(savePath+"/taskList.json", FileMode.OpenOrCreate)))
                    {
                        jsonData = await r.ReadToEndAsync();
                    }
                    return JsonConvert.DeserializeObject<Dictionary<int,CleaningTask>>(jsonData);
            }
    }
}