using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAppCore.BLL.Helpers
{
    public static class Utilities
    {
        private static ILoggerFactory _loggerFactory;

        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public static ILogger CreateLogger<T>()
        {
            if (_loggerFactory == null)
                throw new InvalidOperationException(
                    $"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");

            return _loggerFactory.CreateLogger<T>();
        }

        public static string ToAggregateResult(this IEnumerable<ValidationFailure> errors)
        {
            return JsonConvert.SerializeObject(errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}").ToList());
        }

        public static string SerializeErrorMessage(this Exception exception)
        {
            dynamic errorObject;
            try
            {
                errorObject = JsonConvert.DeserializeObject(exception.Message);
            }
            catch (Exception)
            {
                errorObject = exception.Message;
            }

            return JsonConvert.SerializeObject(new
            {
                messages = errorObject
            });
        }
    }
}