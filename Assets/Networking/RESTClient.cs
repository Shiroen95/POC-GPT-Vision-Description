

public class RESTClient{
    private RESTClient(){

    }

    private static RESTClient _instance;

    public static RESTClient instance {
        get
        {
            if(_instance == null)
                _instance = new RESTClient()

            return _instance;    
        }
    }
    
}