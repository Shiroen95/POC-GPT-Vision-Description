using System;

namespace Config{
    [Serializable]
    ///<summary>
    /// Config data class. Used to import settings from json or other file.
    /// </summary>

    public class APISetting 
    {  
        public string api_base;
        public string deployment_name;
        public string API_KEY;
        public string versionURL;
        public string base_url {
            get {
                return api_base +"openai/deployments/" + deployment_name;
            }
        }
    }
}

