using System;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding;
using AzureFromTheTrenches.Commanding.Abstractions;
using FunctionMonkey.Abstractions.Http;
using FunctionMonkey.Commanding.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;

namespace ToDo
{
    public class HttpResponseHandler : IHttpResponseHandler
    {
        public Task<IActionResult> CreateResponseFromException<TCommand>(TCommand command, Exception ex) where TCommand : ICommand
        {
            // exceptions thrown from inside command handlers are reraised as CommandExecutionException's with the
            // inner exception set to the handler raised exception - so we need to unwrap this to shape our response
            Exception unwrappedException = ex is CommandExecutionException ? ex.InnerException : ex;
            if (unwrappedException is UnauthorizedAccessException)
            {
                return Task.FromResult((IActionResult) new UnauthorizedResult());
            }

            // returning null tells Function Monkey to create its standard response
            return null;
        }

        public Task<IActionResult> CreateResponse<TCommand, TResult>(TCommand command, TResult result) where TCommand : ICommand<TResult>
        {
            return null; // returning null tells Function Monkey to create its standard response
        }

        public Task<IActionResult> CreateResponse<TCommand>(TCommand command)
        {
            return null; // returning null tells Function Monkey to create its standard response
        }

        public Task<IActionResult> CreateValidationFailureResponse<TCommand>(TCommand command, ValidationResult validationResult) where TCommand : ICommand
        {
            return null; // returning null tells Function Monkey to create its standard response
        }
    }
}