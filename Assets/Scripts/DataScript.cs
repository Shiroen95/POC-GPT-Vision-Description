using System.Collections.Generic;
using Networking.DTO;
using UnityEngine;

namespace Scripts {
    public class DataScript 
    {   
        public static Texture2D image {get;set;}
        public static BaseImageDTO request {get;set;} = new BaseImageDTO(){
            max_tokens = 1000,
            messages = new List<GPTRoles>{
                new SystemRole(){
                    content ="You are a helpful assistant."
                    },
                new UserRoleVision(){
                        content = new UserContent[]{
                            new UserTextContent(){
                                text = "Describe this picture: ",
                            },
                            new UserVisionContent(){
                                image_url = new(){
                                    url ="data:image/jpeg;base64,",
                                    detail ="low"
                                }
                            },
                        }
                    }},
        };
        public static BaseResponseDTO response {get;set;} =  new BaseResponseDTO();

        public static (string,string) getResponseMessage{
            get
            {
                var message = response.choices[0].message;
                return ((string)message.content,message.role);
            }
        }
    
    }
}