using Fiksu.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FiksuCore.Web.Http.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        private const string DefaultError = "An unexpected error occurred during the request";
        private const string BadRequestMessage = "Bad request";
        private const string ValidationErrorMessage = "Failed to validate the entity";
        private const string ServerErrorMessage = "Internal server error";
        private const string ForbiddenMessage = "Forbidden";
        private const string UnauthorizedMessage = "Unauthorized";
        private const string NotFoundMessage = "Not found";

        public static IActionResult Success(this HttpResponse res, HttpStatusCode code, object data)
        {
            return new ObjectResult(ApiResponse.Success(code, data));
        }

        private static IActionResult Error(this HttpResponse res, HttpStatusCode code, string defaultMessage, IList<string> errors)
        {
            return new ObjectResult(ApiResponse.Error(code, TransformErrors(defaultMessage, errors)));
        }

        private static IActionResult Error(this HttpResponse res, HttpStatusCode code, string defaultMessage, IList<Exception> errors)
        {
            return new ObjectResult(ApiResponse.Error(code, TransformErrors(defaultMessage, errors)));
        }

        public static IActionResult Created(this HttpResponse res, object result)
        {
            return Success(res, HttpStatusCode.Created, result);
        }

        public static IActionResult Ok(this HttpResponse res, object data)
        {
            return Success(res, HttpStatusCode.OK, data);
        }

        public static IActionResult Error(this HttpResponse res, HttpStatusCode code, IList<Exception> errors)
        {
            return Error(res, code, DefaultError, errors);
        }

        public static IActionResult Error(this HttpResponse res, HttpStatusCode code, params string[] errors)
        {
            return Error(res, code, DefaultError, errors);
        }

        public static IActionResult BadRequest(this HttpResponse res, IList<Exception> errors)
        {
            return Error(res, HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IActionResult BadRequest(this HttpResponse res, IList<string> errors)
        {
            return Error(res, HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IActionResult BadRequest(this HttpResponse res, params string[] errors)
        {
            return Error(res, HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IActionResult Forbidden(this HttpResponse res, IList<Exception> errors)
        {
            return Error(res, HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IActionResult Forbidden(this HttpResponse res, IList<string> errors)
        {
            return Error(res, HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IActionResult Forbidden(this HttpResponse res, params string[] errors)
        {
            return Error(res, HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IActionResult NotFound(this HttpResponse res, IList<Exception> errors)
        {
            return Error(res, HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IActionResult NotFound(this HttpResponse res, IList<string> errors)
        {
            return Error(res, HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IActionResult NotFound(this HttpResponse res, params string[] errors)
        {
            return Error(res, HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IActionResult ServerError(this HttpResponse res, IList<Exception> errors)
        {
            return Error(res, HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IActionResult ServerError(this HttpResponse res, IList<string> errors)
        {
            return Error(res, HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IActionResult ServerError(this HttpResponse res, params string[] errors)
        {
            return Error(res, HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IActionResult Unauthorized(this HttpResponse res, IList<Exception> errors)
        {
            return Error(res, HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IActionResult Unauthorized(this HttpResponse res, IList<string> errors)
        {
            return Error(res, HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IActionResult Unauthorized(this HttpResponse res, params string[] errors)
        {
            return Error(res, HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IActionResult ValidationError(this HttpResponse res, IList<Exception> errors)
        {
            return Error(res, (HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IActionResult ValidationError(this HttpResponse res, IList<string> errors)
        {
            return Error(res, (HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IActionResult ValidationError(this HttpResponse res, params string[] errors)
        {
            return Error(res, (HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        private static string[] TransformErrors(string defaultError, IList<Exception> errors)
        {
            var messages = errors.Select(e => e?.Message)
                .Where(m => !string.IsNullOrEmpty(m))
                .ToArray();

            return messages.Length == 0
                ? new[] { defaultError }
                : messages;
        }

        private static string[] TransformErrors(string defaultError, IList<string> errors)
        {
            var messages = errors.Where(m => !string.IsNullOrEmpty(m))
                .ToArray();

            return messages.Length == 0
                ? new[] { defaultError }
                : messages;
        }
    }
}
