namespace Omnis {
    /// <summary>
    /// Interface for representing an arbitrary execution environment
    /// </summary>
    public interface IExecutionEnvironment {
        /// <summary>
        /// Gets the name of the environment <seealso cref="ExecutionEnvironments"/>
        /// </summary>
        string EnvironmentName { get; }
    }

    /// <summary>
    /// Default implementation of <see cref="IExecutionEnvironment"/>
    /// </summary>
    public class ExecutionEnvironment : IExecutionEnvironment {
        /// <summary>
        /// Gets the name of the environment <seealso cref="ExecutionEnvironments"/>
        /// </summary>
        public string EnvironmentName { get; }

        /// <summary>
        /// Creates a new execution with the given environment name
        /// </summary>
        /// <param name="environmentName"></param>
        public ExecutionEnvironment(string environmentName) {
            EnvironmentName = environmentName;
        }
    }
}
