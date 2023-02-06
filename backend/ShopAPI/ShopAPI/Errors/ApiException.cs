namespace ShopAPI.Errors
{
    public class ApiException : ApiResponse
    {
        public string Details { get; set; }

        public ApiException(string message = null, string details = null)
            : base(500, message)
        {
            Details = details;
        }


    }
}
