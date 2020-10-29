using System.Security.Principal;

namespace Omnis.Web {
    public interface IHttpContext {
        IHttpRequest Request { get; }
        IHttpResponse Response { get; }
        IPrincipal User { get; set; }
    }
}
