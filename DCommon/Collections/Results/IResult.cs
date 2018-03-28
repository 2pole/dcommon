using System;

namespace DCommon
{
    public interface IResult
    {
        bool Success { get; set; }

        string ErrorCode { get; set; }

        string Message { get; set; }
    }
}
