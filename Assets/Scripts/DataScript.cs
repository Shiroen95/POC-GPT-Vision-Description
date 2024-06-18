using System.Collections.Generic;
using Networking.DTO;
using UnityEngine;

namespace Scripts {
    /// <summary>
    /// Static data script for handling I/O.
    /// </summary>
    public class DataScript 
    {   
        public static Texture2D image {get;set;}
        public static BaseImageDTO request {get;set;} = new BaseImageDTO(){
            max_tokens = 1000,
            messages = new List<GPTRoles>{
                new SystemRole(){
                    content ="You are a supervisor that delegates tasks in an cleaning environment. All responses should be in the following json format:"+
                    "{headline:, body:}"+
                    "The content descriptions are as follows: "+
                    "headline: Describes the task at hand as short as possible. "+ 
                    "body: Give instructions on all tasks at hand. If possible, explain how to do these tasks. "+
                    "The Userinput will always be a text and an picture. The text can contain metatags for the task at hand, while the picture will show you the concrete task and it's current state."
                    },
                new UserRoleVision(){
                        content = new UserContent[]{
                            new UserTextContent(){
                                text = "cleaning",
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