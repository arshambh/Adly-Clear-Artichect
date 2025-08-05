namespace Adly.Application.Common;


public interface IOperatorResult
{
    bool IsSuccess { get; set; }
    bool IsNotFound { get; set; }
    List<KeyValuePair<string, string>> ErrorMessages { get; set; }
}


public class OperationResult<TResult> : IOperatorResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public List<KeyValuePair<string, string>> ErrorMessages { get; set; } = [];
    public TResult? Result { get; set; }



    public static OperationResult<TResult> SuccessResult(TResult result)
    {
        return new OperationResult<TResult>
        {
            IsSuccess = true,
            Result = result
        };
    }

    public static OperationResult<TResult> FailureResult(string propertyName, string message)
    {
        var res = new OperationResult<TResult>(){Result = default};
        res.ErrorMessages.Add(new KeyValuePair<string, string>(propertyName, message));
        res.IsSuccess = false;

        return res;
    }

    public static OperationResult<TResult> FailureResult(List<KeyValuePair<string,string>> errors)
    {
        return new OperationResult<TResult>()
        {
            Result = default,
            ErrorMessages = errors
        };
    }

    public static OperationResult<TResult> DomainFailureResult(string errorMessage)
    {
        return new OperationResult<TResult>()
        {
            Result = default,
            ErrorMessages = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("DomainError", errorMessage)
            }
        };
    }

    public static OperationResult<TResult> NotFoundResult(string propertyName, string message)
    {
        var res = new OperationResult<TResult>(){Result = default,IsNotFound = true};
        res.ErrorMessages.Add(new KeyValuePair<string, string>(propertyName, message));
        return res;
    }


}
