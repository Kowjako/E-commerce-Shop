namespace ShopAPI.Errors
{
    public sealed class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorResponse() : base(400) { }
    }
}
