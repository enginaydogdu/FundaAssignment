namespace FundaAssignment.Model
{
    public class Configuration
    {
        public string BaseAddress { get; set; }
        public string Key { get; set; }
        public int PageSize { get; set; }
        public int MaxRetryCount { get; set; }
        public int RetryDuration { get; set; }
    }
}