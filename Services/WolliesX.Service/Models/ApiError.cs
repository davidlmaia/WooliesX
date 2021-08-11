
namespace WolliesX.Service.Models
{
	public class ApiError
	{
        public ApiError()
        {

        }

        public ApiError(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public string Type { get; set; }

        public string Message { get; set; }

        public bool HasError => !string.IsNullOrWhiteSpace(Message);
    }
}
