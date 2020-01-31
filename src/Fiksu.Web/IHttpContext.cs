using System.Security.Principal;

namespace Fiksu.Web {
    public interface IHttpContext {
        IHttpRequest Request { get; }
        IHttpResponse Response { get; }
        IPrincipal User { get; set; }
    }
}
