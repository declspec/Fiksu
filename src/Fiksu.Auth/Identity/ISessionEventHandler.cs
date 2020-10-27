namespace Fiksu.Auth.Identity {
    /// <summary>
    /// Handler for Identity Session lifecycle events in a specific context
    /// <seealso cref="ISessionEventManager{TContext}"/> <seealso cref="ISessionManager{TContext}"/>
    /// </summary>
    /// <typeparam name="TContext">The context that the session is maintained in (i.e. HttpContextBase)</typeparam>
    /// <remarks>
    /// All IIdentitySessionEventHandler instances of a specific generic type are dependency injected into
    /// the parent <see cref="ISessionEventManager{TContext}"/>, this allows a class to hook into
    /// the identity session lifecycle just by implementing the interface (and being registered in the DI container).
    /// This allows a class to clean up any state it's created (i.e. remote sessions) as the lifecycle changes.
    /// </remarks>
    public interface ISessionEventHandler<TContext> {
        /// <summary>
        /// Invoked whenever an <see cref="SessionEvent"/> is raised 
        /// </summary>
        /// <param name="sessionEvent">The session event to handle</param>
        /// <param name="context">The context the event was raised in</param>
        void Handle(SessionEvent sessionEvent, TContext context);
    }
}
