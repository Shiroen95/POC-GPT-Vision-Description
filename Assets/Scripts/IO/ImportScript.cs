using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Networking.DTO;
using Newtonsoft.Json;
using Scripts;
using UnityEngine;


namespace IO
{
    public class ImportScript : MonoBehaviour
    {
        public delegate void OnImportSettings();
        public static event OnImportSettings onImportSettings;

        private string jsonFileType;

        void Start()
        {
            jsonFileType = NativeFilePicker.ConvertExtensionToFileType( "json" ); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        }

        public async void importSetting(){
            var path = pickFile();
            if (path != ""){
                var jsonString = await LoadJson(path);
                DataScript.request = JsonConvert.DeserializeObject<BaseImageDTO>(jsonString);
                onImportSettings?.Invoke();
            }
        }

        private string pickFile(){
            var filePath = "";
            NativeFilePicker.Permission permission = NativeFilePicker.PickFile( ( path ) =>
                {
                    if( path == null )
                        Debug.Log( "Operation cancelled" );
                    else
                        filePath = path;
                }, new string[] { jsonFileType } );

                return filePath;
        }

        public async Task<string> LoadJson(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                return await r.ReadToEndAsync();
            }
        }


    }
}
