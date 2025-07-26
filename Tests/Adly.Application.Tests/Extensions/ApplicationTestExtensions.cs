using Adly.Application.Common;
using Xunit.Abstractions;

namespace Adly.Application.Tests.Extensions;

public static class ApplicationTestExtensions
{

    public static void WritelineOperationResultErrors<TResult>(this ITestOutputHelper testOutputHelper,
        OperationResult<TResult> operationResult)
    {
        foreach (var error in operationResult.ErrorMessages)
        {
            testOutputHelper.WriteLine(
                $"Property Name: {error.Key}, Error Message: {error.Value}");
        }
    }

}