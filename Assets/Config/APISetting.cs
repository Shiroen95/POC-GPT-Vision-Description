using System;

namespace Config{
    [Serializable]
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

