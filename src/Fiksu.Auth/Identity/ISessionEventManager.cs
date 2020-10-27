using System;
using System.Collections.Generic;

namespace Fiksu.Auth.Identity {
    /// <summary>
    /// Aggregates event handlers and manages raising events around the Identity Session lifecycle
    /// </summary>
    /// <typeparam name="TContext">The context that the session is maintained in (i.e. HttpContextBase)</typeparam>
    public interface ISessionEventManager<TContext> {
        /// <summary>
        /// Emit a specific event to all handlers
        /// </summary>
        /// <param name="sessionEvent">Event to emit</param>
        /// <param name="context">The current context</param>
        void Emit(SessionEvent sessionEvent, TContext context);
    }

    public class SessionEventManager<TContext> : ISessionEventManager<TContext> {
        private readonly IEnumerable<ISessionEventHandler<TContext>> _handlers;

        public SessionEventManager(IEnumerable<ISessionEventHandler<TContext>> handlers) {
            _handlers = handlers;
        }

        /// <summary>
        /// Emit a specific event to all handlers
        /// </summary>
        /// <param name="sessionEvent">Event to emit</param>
        /// <param name="context">The current context</param>
        public void Emit(SessionEvent sessionEvent, TContext context) {
            if (_handlers != null) {
                foreach (var handler in _handlers) {
                    try {
                        handler.Handle(sessionEvent, context);
                    }
                    catch (Exception) {
                        // TODO: Look at pulling handlers out of the set if they continually raise exceptions
                        // this may not happen much though.
                    }
                }
            }
        }
    }
}
