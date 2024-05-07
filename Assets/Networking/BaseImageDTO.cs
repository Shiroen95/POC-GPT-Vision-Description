using System.Collections.Generic;

public class BaseImageDTO {

    public List<GPTRoles> messages = new List<GPTRoles>();
    public int max_tokens;
}

public class GPTRoles{
    public string role {get;set;}

}

public class SystemRole: GPTRoles{
     public string content {get;set;}
}

public class UserRoleVision: GPTRoles{
     public UserContent[] content {get;set;}
}

public class UserContent{
    public string type {get;set;}
}


public class UserVisionContent: UserContent{
    public ImageURL image_url {get;set;}
}

public class UserTextContent: UserContent{
    public string text {get;set;}
}
public class ImageURL{
    public string url {get;set;}
    public string detail {get;set;}
}
