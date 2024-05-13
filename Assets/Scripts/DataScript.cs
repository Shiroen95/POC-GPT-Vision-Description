using System.Collections;
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
                new UserRoleVision(){
                        content = new UserContent[]{
                            new UserTextContent(){
                                text = "Describe this picture: ",
                            },
                        }
                    }},
        };
        public static BaseResponseDTO response {get;set;}
    }
}