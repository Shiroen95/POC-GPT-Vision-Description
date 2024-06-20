using System.Collections.Generic;
using Newtonsoft.Json;
namespace Networking.DTO{
    /// <summary>
    /// Datatype thats needed for GPT 4.5 image request.
    /// </summary>
    public class BaseImageDTO {
        public List<GPTRoles> messages = new List<GPTRoles>();
        public int max_tokens;
    }

    public class GPTRoles{
        public string role {get; set;}
        public object content {get;set;}

    }

    public class SystemRole: GPTRoles{
        public SystemRole(){
            role = "system";
        }
        [JsonProperty("content")]
        public new string content {get;set;}
    }

    public class UserRoleVision: GPTRoles{
        public UserRoleVision(){
            role = "user";
        }
        [JsonProperty("content")]
        public new UserContent[] content {get;set;}
    }
    public class ResponseRole: GPTRoles{
        [JsonProperty("content")]
        public new string content {get;set;}
    }

    public class UserContent{
        [JsonProperty("type")]
        public string type {get;protected set;}
    }


    public class UserVisionContent: UserContent{
        [JsonProperty("image_url")]
        public ImageURL image_url {get;set;}
        public UserVisionContent(){
            type = "image_url";
        }
    }

    public class UserTextContent: UserContent{
        public UserTextContent(){
            type = "text";
        }
        [JsonProperty("text")]
        public string text {get; set;}
    }
    public class ImageURL{
        public string url {get;set;}
        public string detail {get;set;}
    }
}
