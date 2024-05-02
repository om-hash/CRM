using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pal.Web.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetError(this Exception ex)
        {
            if (!string.IsNullOrEmpty(ex.InnerException?.Message))
            {
                return ex.InnerException.Message;
            }

            return ex.Message;

        }
    }
}
