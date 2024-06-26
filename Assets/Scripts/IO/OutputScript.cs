using Scripts;
using TMPro;
using UnityEngine;
/// <summary>
/// Output handler script.
/// </summary>
public class OutputScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField responseContent;
    [SerializeField]
    private TMP_Text responseRole;
    [SerializeField]
    private TMP_Text responseTotalToken;
    [SerializeField]
    private TMP_Text responsePromptToken;
    [SerializeField]
    private TMP_Text responseCompletionToken;
    // Start is called before the first frame update
    void Start()
    {
        SetupOutputFields();
    }

    private void SetupOutputFields(){
        responseContent.text = DataScript.getResponseMessage.Item1;
        responseRole.text = DataScript.getResponseMessage.Item2;

        responseTotalToken.text = DataScript.response.usage.total_tokens.ToString();
        responsePromptToken.text =  DataScript.response.usage.prompt_tokens.ToString();
        responseCompletionToken.text =  DataScript.response.usage.completion_tokens.ToString();
    }
}
