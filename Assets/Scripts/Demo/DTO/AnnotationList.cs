using System.Collections.Generic;
using Newtonsoft.Json;

public class AnnotationList{
    [JsonProperty("annotation")]
    public string annotation {get;set;} = "test, test, test";
}