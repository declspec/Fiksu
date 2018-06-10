namespace FiksuClassic.Web.Http
{
    public struct ApiResponse
    {
        public string[] Errors { get; }
        public object Data { get; }
        public int Status { get; }

        public static ApiResponse Success(int status, object data)
        {
            return new ApiResponse(status, data, null);
        }

        public static ApiResponse Error(int status, params string[] errors)
        {
            return new ApiResponse(status, null, errors);
        }

        private ApiResponse(int status, object data, string[] errors)
        {
            Status = status;
            Data = data;
            Errors = errors;
        }
    }
}
