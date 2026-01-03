namespace NetBootcamp.Web.Models;

public struct NoContent;
public class ServiceResponseModel<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public List<string>? FailMessages { get; set; }

    public static ServiceResponseModel<T> Success(T data)
    {
        return new ServiceResponseModel<T>()
        {
            Data = data,
            IsSuccess = true,
        };
    }

    public static ServiceResponseModel<T> Success()
    {
        return new ServiceResponseModel<T>()
        {
            IsSuccess = true
        };
    }

    public static ServiceResponseModel<T> Fail(List<string> failMessages)
    {
        return new ServiceResponseModel<T>()
        {
            IsSuccess = false,
            FailMessages = failMessages
        };
    }

    public static ServiceResponseModel<T> Fail(string failMessage)
    {
        return new ServiceResponseModel<T>()
        {
            IsSuccess = false,
            FailMessages = [failMessage]
        };
    }
}
