using Pal.Data.Contexts;
using Pal.Services.Logger;

using System;
using System.Collections.Generic;
using System.Net;

namespace Pal.Services
{
    public class BaseService<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerService _logger;
        public BaseService(ApplicationDbContext dbContext, ILoggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region Error

        //============//============//============//============//============
        public MyResponseResult Error(Exception ex)
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { ex.GetErrors() },
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Error(Exception ex, HttpStatusCode httpStatus)
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { ex.GetErrors() },
                StatusCode = httpStatus,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Error(string errorMsg)
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { errorMsg },
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Error(string errorMsg, HttpStatusCode httpStatus)
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                Errors = new List<string> { errorMsg },
                StatusCode = httpStatus,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Error(List<string> errorMsgs)
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                Errors = errorMsgs,
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Error(List<string> errorMsgs, HttpStatusCode statusCode)
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                Errors = errorMsgs,
                StatusCode = statusCode,
            };
        }
        #endregion


        #region Success
        //============//============//============//============//============
        public MyResponseResult Success()
        {
            return new MyResponseResult
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Success(HttpStatusCode statusCode)
        {
            return new MyResponseResult
            {
                IsSuccess = true,
                StatusCode = statusCode,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Success(object data)
        {
            return new MyResponseResult
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Data = data,
            };
        }

        //============//============//============//============//============
        public MyResponseResult Success(object data, HttpStatusCode statusCode)
        {
            return new MyResponseResult
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data,
            };
        }

        #endregion


        #region NotFound
        public MyResponseResult NotFound()
        {
            return new MyResponseResult
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
            };
        }
        #endregion

    }

    public class MyResponseResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }
        public object Data { get; set; }

        public static MyResponseResult Success(object data) => new MyResponseResult { IsSuccess = true, Data = data };
        public static MyResponseResult Error(Exception ex, HttpStatusCode httpStatus) => new MyResponseResult { IsSuccess = false, StatusCode = httpStatus, Errors = new List<string> { ex.GetErrors() } };
        public static MyResponseResult Error(List<string> errors, HttpStatusCode httpStatus) => new MyResponseResult { IsSuccess = false, StatusCode = httpStatus, Errors = errors };
    }

    public static class ExeptionExtensions
    {
        public static string GetErrors(this Exception ex)
        {
            if (ex.InnerException != null)
                return ex.InnerException.Message;

            return ex.Message;
        }
    }
}
