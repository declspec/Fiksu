using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Omnis.Web.Http {
    public partial class ApiResponse {
        private const string DefaultError = "An unexpected error occurred during the request";
        private const string BadRequestMessage = "Bad request";
        private const string ValidationErrorMessage = "Failed to validate the entity";
        private const string ServerErrorMessage = "Internal server error";
        private const string ForbiddenMessage = "Forbidden";
        private const string UnauthorizedMessage = "Unauthorized";
        private const string NotFoundMessage = "Not found";

        public static IApiResponse<T> Success<T>(HttpStatusCode code, T data) {
            return new ApiResponse<T>((int)code, data, null);
        }

        public static IApiResponse Success(HttpStatusCode code) {
            return new ApiResponse<object>((int)code, null, null);
        }

        public static IApiResponse Created() {
            return Success(HttpStatusCode.Created);
        }

        public static IApiResponse<T> Created<T>(T result) {
            return Success(HttpStatusCode.Created, result);
        }

        public static IApiResponse Ok() {
            return Success(HttpStatusCode.OK);
        }

        public static IApiResponse<T> Ok<T>(T data) {
            return Success(HttpStatusCode.OK, data);
        }

        public static IApiResponse Error(HttpStatusCode code, IEnumerable<Exception> errors) {
            return Error<object>(code, DefaultError, errors);
        }

        public static IApiResponse Error(HttpStatusCode code, params string[] errors) {
            return Error<object>(code, DefaultError, errors);
        }

        public static IApiResponse<T> Error<T>(HttpStatusCode code, IEnumerable<Exception> errors) {
            return Error<T>(code, DefaultError, errors);
        }

        public static IApiResponse<T> Error<T>(HttpStatusCode code, params string[] errors) {
            return Error<T>(code, DefaultError, errors);
        }

        public static IApiResponse BadRequest(IEnumerable<Exception> errors) {
            return Error(HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IApiResponse BadRequest(IEnumerable<string> errors) {
            return Error(HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IApiResponse BadRequest(params string[] errors) {
            return Error(HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IApiResponse<T> BadRequest<T>(IEnumerable<Exception> errors) {
            return Error<T>(HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IApiResponse<T> BadRequest<T>(IEnumerable<string> errors) {
            return Error<T>(HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IApiResponse<T> BadRequest<T>(params string[] errors) {
            return Error<T>(HttpStatusCode.BadRequest, BadRequestMessage, errors);
        }

        public static IApiResponse Forbidden(IEnumerable<Exception> errors) {
            return Error(HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IApiResponse Forbidden(IEnumerable<string> errors) {
            return Error(HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IApiResponse Forbidden(params string[] errors) {
            return Error(HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IApiResponse<T> Forbidden<T>(IEnumerable<Exception> errors) {
            return Error<T>(HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IApiResponse<T> Forbidden<T>(IEnumerable<string> errors) {
            return Error<T>(HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IApiResponse<T> Forbidden<T>(params string[] errors) {
            return Error<T>(HttpStatusCode.Forbidden, ForbiddenMessage, errors);
        }

        public static IApiResponse NotFound(IEnumerable<Exception> errors) {
            return Error(HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IApiResponse NotFound(IEnumerable<string> errors) {
            return Error(HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IApiResponse NotFound(params string[] errors) {
            return Error(HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IApiResponse<T> NotFound<T>(IEnumerable<Exception> errors) {
            return Error<T>(HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IApiResponse<T> NotFound<T>(IEnumerable<string> errors) {
            return Error<T>(HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IApiResponse<T> NotFound<T>(params string[] errors) {
            return Error<T>(HttpStatusCode.NotFound, NotFoundMessage, errors);
        }

        public static IApiResponse ServerError(IEnumerable<Exception> errors) {
            return Error(HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IApiResponse ServerError(IEnumerable<string> errors) {
            return Error(HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IApiResponse ServerError(params string[] errors) {
            return Error(HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IApiResponse<T> ServerError<T>(IEnumerable<Exception> errors) {
            return Error<T>(HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IApiResponse<T> ServerError<T>(IEnumerable<string> errors) {
            return Error<T>(HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IApiResponse<T> ServerError<T>(params string[] errors) {
            return Error<T>(HttpStatusCode.InternalServerError, ServerErrorMessage, errors);
        }

        public static IApiResponse Unauthorized(IEnumerable<Exception> errors) {
            return Error(HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IApiResponse Unauthorized(IEnumerable<string> errors) {
            return Error(HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IApiResponse Unauthorized(params string[] errors) {
            return Error(HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IApiResponse<T> Unauthorized<T>(IEnumerable<Exception> errors) {
            return Error<T>(HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IApiResponse<T> Unauthorized<T>(IEnumerable<string> errors) {
            return Error<T>(HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IApiResponse<T> Unauthorized<T>(params string[] errors) {
            return Error<T>(HttpStatusCode.Unauthorized, UnauthorizedMessage, errors);
        }

        public static IApiResponse ValidationError(IEnumerable<Exception> errors) {
            return Error((HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IApiResponse ValidationError(IEnumerable<string> errors) {
            return Error((HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IApiResponse ValidationError(params string[] errors) {
            return Error((HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IApiResponse<T> ValidationError<T>(IEnumerable<Exception> errors) {
            return Error<T>((HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IApiResponse<T> ValidationError<T>(IEnumerable<string> errors) {
            return Error<T>((HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        public static IApiResponse<T> ValidationError<T>(params string[] errors) {
            return Error<T>((HttpStatusCode)422, ValidationErrorMessage, errors);
        }

        private static IApiResponse Error(HttpStatusCode code, string defaultMessage, IEnumerable<string> errors) {
            return new ApiResponse<object>((int)code, null, TransformErrors(defaultMessage, errors));
        }

        private static IApiResponse Error(HttpStatusCode code, string defaultMessage, IEnumerable<Exception> errors) {
            return new ApiResponse<object>((int)code, null, TransformErrors(defaultMessage, errors));
        }

        private static IApiResponse<T> Error<T>(HttpStatusCode code, string defaultMessage, IEnumerable<string> errors) {
            return new ApiResponse<T>((int)code, default(T), TransformErrors(defaultMessage, errors));
        }

        private static IApiResponse<T> Error<T>(HttpStatusCode code, string defaultMessage, IEnumerable<Exception> errors) {
            return new ApiResponse<T>((int)code, default(T), TransformErrors(defaultMessage, errors));
        }

        private static IList<string> TransformErrors(string defaultError, IEnumerable<Exception> errors) {
            var messages = errors?.Select(e => e?.Message)
                .Where(m => !string.IsNullOrEmpty(m))
                .ToList();

            return messages?.Count > 0 ? messages : new List<string>() { defaultError };
        }

        private static IList<string> TransformErrors(string defaultError, IEnumerable<string> errors) {
            var messages = errors?.Where(m => !string.IsNullOrEmpty(m))
                .ToList();

            return messages?.Count > 0 ? messages : new List<string>() { defaultError };
        }
    }
}