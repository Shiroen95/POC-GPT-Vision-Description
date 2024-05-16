namespace Networking.DTO{
    /// <summary>
    /// Response from GPT4.5 Image request.
    /// </summary>
    public class BaseResponseDTO{
        public string id {get;set;}
        public string _object {get;set;}
        public int created {get;set;}
        public string model {get;set;}
        public PromptFilterResult[] prompt_filter_results {get;set;}
        public Choice[] choices {get;set;}
        public Usage usage {get;set;}
        
    }

    public class PromptFilterResult{
        public int prompt_index {get;set;}
        public ContentFilterResults content_filter_results {get;set;}
    }

    public class ContentFilterResult{
        public bool filtered {get;set;}
        public string severity {get;set;}
    }

    public class ContentFilterResults{
        public ContentFilterResult hate {get;set;}
        public ContentFilterResult self_harm {get;set;}
        public ContentFilterResult sexual {get;set;}
        public ContentFilterResult violence {get;set;}
    }

    public class Choice{
        public string finish_reason {get;set;}
        public int index {get;set;}
        public GPTRoles message {get;set;}
        public ContentFilterResults content_filter_results {get;set;}
    }

    public class Usage{
        public int prompt_tokens {get;set;}
        public int completion_tokens {get;set;}
        public int total_tokens {get;set;}
    }

}