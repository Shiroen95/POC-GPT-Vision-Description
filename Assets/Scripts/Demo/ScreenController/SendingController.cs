using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo;
using UnityEngine;


public class SendingController : MonoBehaviour
{
    private int currStep = 0;
    private Dictionary<InteractionMode,RequestMode[]> interactionRoutes = new Dictionary<InteractionMode, RequestMode[]>
    {
            {
                InteractionMode.baseMethod, null
            },
            {
                InteractionMode.semi1Method, new [] {RequestMode.task}
            },
            {
                InteractionMode.semi2Method, new [] {RequestMode.userTags,RequestMode.task}   
            },
            {
                InteractionMode.fullAutoMethod, new [] {RequestMode.autoTags,RequestMode.task}
            }   
    };

    public async Task<string> requestStep(){
        DemoDataScript.Instance.currentRequestMode = interactionRoutes[DemoDataScript.Instance.currentInteractionMode][currStep];
        currStep++; 
        return await sendRequestAsync();  
    }

    private async Task<string> sendRequestAsync(){ 
            var response = await RESTClient.instance.returnGPT4PostRequest(
                Convert.ToBase64String(
                    DemoDataScript.Instance.currImage?.EncodeToJPG()),
                DemoDataScript.Instance.request);
            return (string)response.choices[0].message.content;
    }
}
