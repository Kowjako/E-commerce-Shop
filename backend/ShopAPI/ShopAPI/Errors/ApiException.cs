namespace ShopAPI.Errors
{
    public sealed class ApiException : ApiResponse
    {
        public string Details { get; set; }

        public ApiException(string message = null, string details = null)
            : base(500, message)
        {
            Details = details;
        }
    }
}
