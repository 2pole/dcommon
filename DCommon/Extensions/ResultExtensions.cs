using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon
{
    public static class ResultExtensions
    {
        public static IResult<T> Convert<T>(this IResult result)
        {
            return new Result<T> { Success = result.Success, Message = result.Message, ErrorCode = result.ErrorCode };
        }

        public static IResult<T> Convert<T>(this IResult result, T data)
        {
            var r = result.Convert<T>();
            r.Data = data;
            return r;
        }

        public static IResult Convert(this IResult result)
        {
            return new Result { Success = result.Success, Message = result.Message, ErrorCode = result.ErrorCode };
        }

        public static IListResult<T> ToList<T>(this IResult result)
        {
            return new ListResult<T> { Success = result.Success, Message = result.Message, ErrorCode = result.ErrorCode };
        }

        public static IListResult<T> ToList<T>(this IResult result, IList<T> data)
        {
            var r = result.ToList<T>();
            r.Data = data;
            return r;
        }
    }
}
