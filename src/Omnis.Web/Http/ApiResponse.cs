using System.Collections.Generic;

namespace Omnis.Web.Http {
    public interface IApiResponse {
        int Status { get; }
        IList<string> Errors { get; }
    }

    public interface IApiResponse<out T> : IApiResponse {
        T Data { get; }
    }

    public partial class ApiResponse : IApiResponse {
        public int Status { get; }
        public IList<string> Errors { get; }

        public ApiResponse(int status, IList<string> errors) {
            Status = status;
            Errors = errors;
        }
    }

    public class ApiResponse<T> : ApiResponse, IApiResponse<T> {
        public T Data { get; }
       
        public ApiResponse(int status, T data, IList<string> errors) : base(status, errors) {
            Data = data;
        }
    }
}
