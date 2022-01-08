using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Lab.Entities
{
    public class OperationResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public OperationResult()
        {
            ErrorMessage = string.Empty;
            HttpStatusCode = HttpStatusCode.OK;
        }

        public bool IsSuccess()
        {
            var httpStatusCode = (int)HttpStatusCode;
            return httpStatusCode is >= 200 and < 300;
        }

        public OperationResult(HttpStatusCode httpStatusCode, string errorMessage)
        {
            HttpStatusCode = httpStatusCode;
            ErrorMessage = errorMessage;
        }

        public virtual IActionResult ToResponseMessage()
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                return new NoContentResult();
            }

            return new JsonResult(new { Error = ErrorMessage })
            {
                StatusCode = (int)HttpStatusCode
            };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }


        public override IActionResult ToResponseMessage()
        {
            if (!string.IsNullOrEmpty(ErrorMessage)) return base.ToResponseMessage();

            return new JsonResult(Value)
            {
                StatusCode = (int)HttpStatusCode
            };
        }

        public OperationResult(HttpStatusCode httpStatusCode, string errorMessage) : base(httpStatusCode, errorMessage)
        {
        }

        public OperationResult(T value) : base(HttpStatusCode.OK, string.Empty)
        {
            Value = value;
        }
    }
}