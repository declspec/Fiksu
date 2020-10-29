using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace OmnisClassic.Web.Extensions {
    public static class ModelStateExtensions {
        public static IList<string> GetAllErrors(this ModelStateDictionary modelState) {
            return modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }
    }
}
