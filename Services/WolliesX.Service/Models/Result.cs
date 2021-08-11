namespace WolliesX.Service.Models
{
	public class Result<T>
	{
        public bool Success => (Error == null || Error.HasError == false) && Value != null;

        public ApiError Error { get; }

        public T Value { get; }

        public Result(ApiError error)
        {
            Error = error;
        }

        public Result(T value)
        {
            Value = value;
        }
    }
}
