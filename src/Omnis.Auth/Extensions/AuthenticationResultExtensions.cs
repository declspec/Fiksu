using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omnis.Auth.Extensions {
    internal static class AuthenticationResultExtensions {
        public static async Task<AuthenticationResult> AggregateResultsAsync<TProvider>(this IEnumerable<TProvider> providers, Func<TProvider, Task<AuthenticationResult>> resultResolver) {
            // If no providers are supplied, return an empty unsuccessful result.
            if (providers == null)
                return AuthenticationResult.Skip;

            var failureResults = new HashSet<AuthenticationResult>();

            // TODO: Serial vs parallel?
            foreach (var provider in providers) {
                var result = await resultResolver(provider).ConfigureAwait(false);

                if (result == null || result.Skipped)
                    continue;

                if (result.Successful)
                    return result;

                failureResults.Add(result);
            }

            switch (failureResults.Count) {
                case 0: return AuthenticationResult.Skip;
                case 1: return failureResults.First();
                default:
                    // Aggregate all the error messages into a new AuthenticationResult
                    var errors = failureResults.SelectMany(r => r.Errors).Distinct().ToList();
                    return errors.Count == 0 ? AuthenticationResult.Skip : AuthenticationResult.Failure(errors);
            }
        }
    }
}
