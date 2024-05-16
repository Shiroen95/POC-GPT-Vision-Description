using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Config;
using Networking.DTO;
using System.Threading.Tasks;
using Scripts;

/// <summary>
/// REST Client to send GPT4.5 Requests. API Settings need to be Present to work. 
/// </summary>
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
    private BaseImageDTO addImageToRequest(string base64Image, BaseImageDTO imageDTO){
        var userVisionRole =  (UserRoleVision)imageDTO.messages[1];
        var content = userVisionRole.content;
        ((UserVisionContent) content[1]).image_url.url = "data:image/jpeg;base64," + base64Image;
        return DataScript.request;                    
    }
    public async Task sendGPT4PostRequest(string base64Image, BaseImageDTO imageDTO = null){
        var endpoint = settings.base_url + settings.versionURL;
        if(imageDTO == null){
            imageDTO = DataScript.request;
        }
        addImageToRequest(base64Image, imageDTO); 
        var response = await client.PostAsync(endpoint, 
        new StringContent(
            JsonConvert.SerializeObject(imageDTO), 
            Encoding.UTF8,
            "application/json"));
       var stringContent = await response.Content.ReadAsStringAsync();     
       DataScript.response = JsonConvert.DeserializeObject<BaseResponseDTO>(stringContent);  
       UnityEngine.Debug.Log(stringContent);
       UnityEngine.Debug.Log("Done!"); 

    }
}
