using Newtonsoft.Json;

namespace Demo.DTO{
    public class CleaningTask{
        
        public CleaningTask(){}

        public CleaningTask(string Name, string Description){
            this.Name = Name;
            this.Description = Description;
        }
        [JsonProperty("headline")]
        public string Name{get;set;}
        [JsonProperty("body")]
        public string Description{get;set;}

    }
}