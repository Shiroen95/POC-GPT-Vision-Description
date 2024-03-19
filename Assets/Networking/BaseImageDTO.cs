using System.Collections.Generic;

public class BaseImageDTO {

    public List<GPTRoles> message = new List<GPTRoles>();
    public int max_tokens;
}

public class GPTRoles{
    string role {get;set;}
    string content {get;set;}
}

public class UserContent{
    string type {get;set;}
    string text {get;set;}
    string image_url {get;set;}
}
