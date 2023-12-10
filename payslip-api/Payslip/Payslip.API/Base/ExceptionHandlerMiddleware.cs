using FluentValidation;
using Newtonsoft.Json;
using Payslip.API.Extensions;
using Payslip.Application.Base;
using System.Net;

namespace Payslip.API.Base
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestUrl = context.Request.GetFullUrl();

            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.BadRequest, exception.Message);
            }
            catch (NotFoundException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.NotFound, exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.Unauthorized, exception.Message);
            }
            catch (ManagedException exception)
            {
                await ConfigureResponse(context, HttpStatusCode.BadRequest, exception.Message);
            }
            catch (Exception exception)
            {
                await ConfigureResponse(context, HttpStatusCode.InternalServerError, "متاسفانه خطای سیستمی رخ داده است، در صورت لزوم با پشتیبانی تماس حاصل نمایید");
            }
        }

        private static async Task ConfigureResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                new FailedResponseMessage(message).ToString());
        }

    }

    public class FailedResponseMessage
    {
        public FailedResponseMessage(string message)
        {
            this.message = message;
        }
        public string message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
