using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Tarqeem.CA.Application.Models.Common;

public class OperationResult<TResult>
{
    public TResult Result { get; private set; }

    public bool IsSuccess { get; private set; }
    public string ErrorCode { get; private set; }
    public bool IsException { get; set; }
    public bool IsNotFound { get; private set; }

    public static OperationResult<TResult> SuccessResult(TResult result)
    {
        return new OperationResult<TResult> { Result = result, IsSuccess = true };
    }

    public static OperationResult<TResult> FailureResult(
        RawdahErrors errorCode,
        string errorMessage = null,
        ILogger logger = null,
        TResult result = default
    )
    {
        if (!errorMessage.IsNullOrEmpty() && logger != null)
            logger.LogError(errorMessage);
        return new OperationResult<TResult>
        {
            Result = result,
            ErrorCode = errorCode.ToString(),
            IsSuccess = false,
        };
    }

    public static OperationResult<TResult> NotFoundResult()
    {
        return new OperationResult<TResult> { IsSuccess = false, IsNotFound = true };
    }
}

[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
public enum RawdahErrors
{
    EmptyPassword,
    ServerError,
    UsernameExists,
    LockedOut,
    UsernameNotFound,
    PasswordInvalid,
    UserNotFound,
    EmptyUsername,
    OrganizationNotFound,
    RoomNotFound,
    InvalidFileType,
    FormatNotSupported,
    AlreadyAdded,
    NotIn,
    StudentNotFound,
}
