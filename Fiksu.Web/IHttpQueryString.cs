using Microsoft.Extensions.Primitives;

namespace Fiksu.Web
{
    public interface IHttpQueryString
    {
        StringValues this[string index] { get; }
    }
}
