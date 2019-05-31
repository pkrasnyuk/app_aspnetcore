using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using WebAppCore.BLL.Exceptions;
using WebAppCore.BLL.Helpers;
using WebAppCore.DAL.Interfaces;
using WebAppCore.Domain.Entities;

namespace WebAppCore.BLL.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEntityRepository<Error<ObjectId>, ObjectId> _errorRepository;

        public ErrorHandlingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _errorRepository = (IEntityRepository<Error<ObjectId>, ObjectId>) serviceProvider.GetService(
                typeof(IEntityRepository<Error<ObjectId>, ObjectId>));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await _errorRepository.CreateEntityAsync(new Error<ObjectId>
                {
                    StackTrace = ex.StackTrace,
                    Message = ex.Message
                });
                await HandleExceptionAsync(context, ex);
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                string result = null;

                if (context.Response.StatusCode == (int) HttpStatusCode.Unauthorized)
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        messages = "You are not authorized to access this resource"
                    });
                }
                if (context.Response.StatusCode == (int) HttpStatusCode.Forbidden)
                {
                    result = JsonConvert.SerializeObject(new
                    {
                        messages = "You are not have permission for modify this resource"
                    });
                }

                await context.Response.WriteAsync(result);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case ResourceSearchException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case PermissionsException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case BadRequestException _:
                    code = HttpStatusCode.BadRequest;
                    break;
                case ConflictRequestException _:
                    code = HttpStatusCode.Conflict;
                    break;
            }
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(exception.SerializeErrorMessage());
        }
    }
}