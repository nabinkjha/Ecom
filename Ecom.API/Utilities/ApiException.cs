using System.Text.Json;

namespace ECom.API.Utilities
{
    public class ApiException
    {
        public ApiException(int statusCode, string errorMessage, string errorDetails=null)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
        }

        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
