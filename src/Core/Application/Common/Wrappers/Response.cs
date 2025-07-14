namespace Application.Common.Wrappers
{
    public class Response<T>
    {
        public Response()
        {

        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public Response(bool succeded, string message)
        {
            Succeeded = succeded;
            Message = message;
        }
        public Response(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

    }
}