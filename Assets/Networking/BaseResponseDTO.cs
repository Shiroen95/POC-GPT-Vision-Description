namespace Networking.DTO{
    public class BaseResponseDTO{
        public string id;
        public string _object;
        public int created;
        public string model;
        public PromptFilterResult[] prompt_filter_results;
        public Choice[] choices;
        public Usage usage;
        
    }

    public class PromptFilterResult{
        public int prompt_index;
        public ContentFilterResults content_filter_results;
    }

    public class ContentFilterResult{
        public bool filtered;
        public string severity;
    }

    public class ContentFilterResults{
        public ContentFilterResult hate;
        public ContentFilterResult self_harm;
        public ContentFilterResult sexual;
        public ContentFilterResult violence;
    }

    public class Choice{
        public string finish_reason;
        public int index;
        public GPTRoles message;
        public ContentFilterResults content_filter_results;
    }

    public class Usage{
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }

}