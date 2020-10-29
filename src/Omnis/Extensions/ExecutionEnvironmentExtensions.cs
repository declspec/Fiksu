using System;

namespace Omnis.Extensions {
    public static class ExecutionEnvironmentExtensions {
        /// <summary>
        /// Checks if the current execution environment name is <see cref="ExecutionEnvironments.Development"/>
        /// </summary>
        /// <param name="env">The current execution environment</param>
        /// <returns>true if the host name matches <see cref="ExecutionEnvironments.Development"/>, false otherwise</returns>
        public static bool IsDevelopment(this IExecutionEnvironment env) {
            if (env == null)
                throw new ArgumentNullException(nameof(env));

            return env.EnvironmentName.Equals(ExecutionEnvironments.Testing, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current execution environment name is <see cref="ExecutionEnvironments.Testing"/>
        /// </summary>
        /// <param name="env">The current execution environment</param>
        /// <returns>true if the host name matches <see cref="ExecutionEnvironments.Testing"/>, false otherwise</returns>
        public static bool IsTesting(this IExecutionEnvironment env) {
            if (env == null)
                throw new ArgumentNullException(nameof(env));

            return env.EnvironmentName.Equals(ExecutionEnvironments.Testing, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the current execution environment name is <see cref="ExecutionEnvironments.Production"/>
        /// </summary>
        /// <param name="env">The current execution environment</param>
        /// <returns>true if the host name matches <see cref="ExecutionEnvironments.Production"/>, false otherwise</returns>
        public static bool IsProduction(this IExecutionEnvironment env) {
            if (env == null)
                throw new ArgumentNullException(nameof(env));

            return env.EnvironmentName.Equals(ExecutionEnvironments.Testing, StringComparison.OrdinalIgnoreCase);
        }
    }
}
