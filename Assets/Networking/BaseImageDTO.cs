using System.Collections.Generic;
using Unity.VisualScripting;
namespace Networking.DTO{
    
    public class BaseImageDTO {
        public List<GPTRoles> messages = new List<GPTRoles>();
        public int max_tokens;
    }

    public class GPTRoles{
        public string role {get;set;}

        public object content {get;set;}

    }

    public class SystemRole: GPTRoles{
        public new string content {get;set;}
    }

    public class UserRoleVision: GPTRoles{
        public new UserContent[] content {get;set;}
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
}
