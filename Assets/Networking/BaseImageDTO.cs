using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
namespace Networking.DTO{
    
    public class BaseImageDTO {
        public List<GPTRoles> messages = new List<GPTRoles>();
        public int max_tokens;
    }

    public class GPTRoles{
        public string role {get;protected set;}

        public object content {get;set;}

    }

    public class SystemRole: GPTRoles{
        public SystemRole(){
            role = "system";
        }
        public new string content {get;set;}
    }

    public class UserRoleVision: GPTRoles{
        public UserRoleVision(){
            role = "user";
        }
        public new UserContent[] content {get;set;}
    }

    public class UserContent{
        public string type {get;protected set;}
    }


    public class UserVisionContent: UserContent{
        public ImageURL image_url {get;set;}
        public UserVisionContent(){
            type = "image_url";
        }
    }

    public class UserTextContent: UserContent{
        public UserTextContent(){
            type = "text";
        }
        public string text {get; set;}
    }
    public class ImageURL{
        public string url {get;set;}
        public string detail {get;set;}
    }
}
