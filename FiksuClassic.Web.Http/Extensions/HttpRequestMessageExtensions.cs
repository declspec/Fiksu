using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace FiksuClassic.Web.Http.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        private const string DefaultError = "An unexpected error occurred during the request";
        private const string BadRequestMessage = "Bad request";
        private const string ValidationErrorMessage = "Failed to validate the entity";
        private const string ServerErrorMessage = "Internal server error";
        private const string ForbiddenMessage = "Forbidden";
        private const string UnauthorizedMessage = "Unauthorized";
        private const string NotFoundMessage = "Not found";

        public static HttpResponseMessage Success(this HttpRequestMessage req, HttpStatusCode code, object data)
        {
            return req.CreateResponse(code, ApiResponse.Success((int)code, data));
        }

        public static HttpResponseMessage Created(this HttpRequestMessage req, object result)
        {
            return Success(req, HttpStatusCode.Created, result);
        }

        public static HttpResponseMessage Ok(this HttpRequestMessage req, object data)
        {
            return Success(req, HttpStatusCode.OK, data);
        }

        public static HttpResponseMessage Error(this HttpRequestMessage req, HttpStatusCode code, IList<Exception> errors)
        {
            return Error(req, code, DefaultError, errors);
        }

        public static HttpResponseMessage Error(this HttpRequestMessage req, HttpStatusCode code, params string[] errors)
        {
            return Error(req, code, DefaultError, errors);
        }

        public static HttpResponseMessage BadRequest(this HttpRequestMessage req, IList<Exception> errors)
        {
            return Error(req, HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static HttpResponseMessage BadRequest(this HttpRequestMessage req, IList<string> errors)
        {
            return Error(req, HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static HttpResponseMessage BadRequest(this HttpRequestMessage req, params string[] errors)
        {
            return Error(req, HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static HttpResponseMessage Forbidden(this HttpRequestMessage req, IList<Exception> errors)
        {
            return Error(req, HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static HttpResponseMessage Forbidden(this HttpRequestMessage req, IList<string> errors)
        {
            return Error(req, HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static HttpResponseMessage Forbidden(this HttpRequestMessage req, params string[] errors)
        {
            return Error(req, HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static HttpResponseMessage NotFound(this HttpRequestMessage req, IList<Exception> errors)
        {
            return Error(req, HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static HttpResponseMessage NotFound(this HttpRequestMessage req, IList<string> errors)
        {
            return Error(req, HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static HttpResponseMessage NotFound(this HttpRequestMessage req, params string[] errors)
        {
            return Error(req, HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static HttpResponseMessage ServerError(this HttpRequestMessage req, IList<Exception> errors)
        {
            return Error(req, HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static HttpResponseMessage ServerError(this HttpRequestMessage req, IList<string> errors)
        {
            return Error(req, HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static HttpResponseMessage ServerError(this HttpRequestMessage req, params string[] errors)
        {
            return Error(req, HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static HttpResponseMessage Unauthorized(this HttpRequestMessage req, IList<Exception> errors)
        {
            return Error(req, HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static HttpResponseMessage Unauthorized(this HttpRequestMessage req, IList<string> errors)
        {
            return Error(req, HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static HttpResponseMessage Unauthorized(this HttpRequestMessage req, params string[] errors)
        {
            return Error(req, HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static HttpResponseMessage ValidationError(this HttpRequestMessage req, IList<Exception> errors)
        {
            return Error(req, (HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static HttpResponseMessage ValidationError(this HttpRequestMessage req, IList<string> errors)
        {
            return Error(req, (HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static HttpResponseMessage ValidationError(this HttpRequestMessage req, params string[] errors)
        {
            return Error(req, (HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        private static HttpResponseMessage Error(this HttpRequestMessage req, HttpStatusCode code, string defaultMessage, IList<string> errors)
        {
            return req.CreateResponse(code, ApiResponse.Error((int)code, TransformErrors(defaultMessage, errors)));
        }

        private static HttpResponseMessage Error(this HttpRequestMessage req, HttpStatusCode code, string defaultMessage, IList<Exception> errors)
        {
            return req.CreateResponse(code, ApiResponse.Error((int)code, TransformErrors(defaultMessage, errors)));
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
