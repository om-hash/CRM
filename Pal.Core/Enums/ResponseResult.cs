using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Core.Enums
{
    public enum ResponseType
    {
        Error,
        Success,
        HasTransactions,
        ModelNotValid,
    }
    public class ResponseResult
    {

        public ResponseResult(ResponseType responseType)
        {
            ResponseType = responseType;
        }

        public ResponseResult(ResponseType responseType, int statusCode)
        {
            ResponseType = responseType;
            StatusCode = statusCode;
        }

        public ResponseResult(ResponseType responseType, string responseText)
        {
            ResponseType = responseType;
            ResponseText = responseText;
        }

        public ResponseResult(ResponseType responseType, dynamic result)
        {
            ResponseType = responseType;
            Result = result;
        }

        public ResponseResult(ResponseType responseType, string responseText, dynamic result)
        {
            ResponseType = responseType;
            ResponseText = responseText;
            Result = result;
        }

        public ResponseResult(ResponseType responseType, dynamic result, int statusCode)
        {
            ResponseType = responseType;
            StatusCode = statusCode;
            Result = result;
        }

        public ResponseResult(ResponseType responseType, string responseText, int statusCode)
        {
            ResponseType = responseType;
            StatusCode = statusCode;
            ResponseText = responseText;
        }

        public int? StatusCode { get; set; }

        public ResponseType ResponseType { get; set; }

        [StringLength(100)]
        public string ResponseText { get; set; }

        public dynamic Result { get; set; }
    }
}
