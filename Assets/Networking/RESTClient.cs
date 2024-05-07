using System.Net.Http;
using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Config;




public class RESTClient {
    public static APISetting settings = ConfigLoader.LoadConfig();
    
    private static readonly HttpClient client = new HttpClient();
    
    private RESTClient(){
        client.DefaultRequestHeaders.Add("api-key",settings.API_KEY);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private static RESTClient _instance;

    public static RESTClient instance {
        get
        {
            if(_instance == null)
                _instance = new RESTClient();

            return _instance;    
        }
    }

    public async void sendGPT4PostRequest(string base64Image){
        var endpoint = settings.base_url + settings.versionURL;
        BaseImageDTO imageDTO = new BaseImageDTO
        {
            max_tokens = 1000
        };
        imageDTO.messages.AddRange(
            new GPTRoles[]{
                new SystemRole(){
                   role = "system",
                   content ="You are a helpful assistant."
                },
                new UserRoleVision(){
                    role = "user",
                    content = new UserContent[]{
                        new UserTextContent(){
                            type = "text",
                            text = "Describe this picture: ",
                        },
                        new UserVisionContent(){
                            type ="image_url",
                            image_url = new(){
                                url ="data:image/jpeg;base64," + base64Image,
                                detail ="low"
                            }
                        }
                    }
                }
            }
        );
        Debug.Log(JsonConvert.SerializeObject(imageDTO));
        Debug.Log(client.DefaultRequestHeaders.ToString());
        var response = await client.PostAsync(endpoint, 
        new StringContent(
            JsonConvert.SerializeObject(imageDTO), 
            Encoding.UTF8,
            "application/json"));
        Debug.Log(await response.Content.ReadAsStringAsync());
    }
    
   
}
