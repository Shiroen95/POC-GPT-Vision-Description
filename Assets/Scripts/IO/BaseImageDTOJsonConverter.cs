using System;
using System.Collections.Generic;
using System.Linq;
using Networking.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BaseImageDTOJsonConverter : JsonConverter<BaseImageDTO>
{

    public override BaseImageDTO ReadJson(JsonReader reader, Type objectType, BaseImageDTO existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        
        BaseImageDTO DTO = new BaseImageDTO();
        List<GPTRoles> messages = new List<GPTRoles>();
        JObject jObject = JObject.Load(reader);
        DTO.max_tokens = (int)jObject["max_tokens"];
        var jMessage = jObject["messages"];
        foreach (var message in jMessage.Children())
        {
            var role = (string)message["role"];
           if("system" == role){
                messages.Add(message.ToObject<SystemRole>());
           }
           else if("user" == role){
                var userVisionRole = new UserRoleVision();
                var visionContent = message["content"];
                userVisionRole.content = new UserContent[visionContent.Children().Count()];
                int i = 0;
                foreach (var content in visionContent.Children()){
                    var type = (string)content["type"];
                    if(type == "image_url"){
                        userVisionRole.content[i] = content.ToObject<UserVisionContent>();
                    }
                    else if(type ==  "text"){
                        userVisionRole.content[i] = content.ToObject<UserTextContent>();
                    }
                    i++;
                }
                messages.Add(userVisionRole);
           }
        }
        DTO.messages = messages;
        return DTO;
    }


    public override void WriteJson(JsonWriter writer, BaseImageDTO value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
