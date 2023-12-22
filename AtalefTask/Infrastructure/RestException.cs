using Newtonsoft.Json;
using System.Net;

namespace AtalefTask.Infrastructure
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public new string Message { get; set; }
        public RestException(HttpStatusCode code, string message)
        {
            Code = code;
            Message = message;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class NotFoundException : RestException
    {
        public NotFoundException(string message) : base(HttpStatusCode.NotFound, message)
        {       
        }
    }

    public class ConflictException : RestException
    {
        public ConflictException(string message) : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
