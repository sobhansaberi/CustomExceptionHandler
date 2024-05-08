using Newtonsoft.Json;

namespace Application.Common.Exceptions;

public class AppException : Exception
{
    public AppException() : base()
    {
        Code = 0;
        Target = string.Empty;
    }

    public AppException(int code, string message, string target)
    {
        Code = code;
        Message = message;
        Target = target;
    }

    public int Code { get; set; }
    public new string Message { get; set; }
    public string Target { get; set; }
    public new string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}
