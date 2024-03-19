using System.Net.Http;

public class RESTClient {
    private static readonly HttpClient client = new HttpClient();
    
    private RESTClient(){

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

    public void sendGPT4PostRequest(string URI){
      
    }

    
    
}