using System;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Proxy;

namespace CSF.NHibernate
{
    /// <summary>
    /// An implementation of an NHibernate unproxying service, which can
    /// 'unwrap' an NHibernate proxy object and return a representation
    /// of the same object as its original type, without proxying.
    /// </summary>
    public class ObjectUnproxyingService : IUnproxiesObject
    {
        readonly ISession session;

        /// <summary>
        /// 'Unproxy' the specified object and get a representation of the same
        /// object which is not an NHibernate proxy.
        /// </summary>
        /// <returns>A representation of the same <paramref name="obj"/>, which is not an NHibernate proxy.</returns>
        /// <param name="obj">The object to 'unproxy'.</param>
        /// <typeparam name="T">The object type.</typeparam>
        public T Unproxy<T>(T obj) where T : class
        {
            if (obj is null) return null;

            if (!NHibernateUtil.IsInitialized(obj))
                NHibernateUtil.Initialize(obj);

            if (!(obj is INHibernateProxy))
                return obj;

            var context = GetPersistenceContext();
            return (T) context.Unproxy(obj);
        }

        /// <summary>
        /// Gets a persistence context.  This implementation will raise an exception if the context is null.
        /// </summary>
        /// <returns>The persistence context.</returns>
        /// <exception cref="InvalidOperationException">If no context can be retrieved.</exception>
        protected virtual IPersistenceContext GetPersistenceContext()
        {
            var context = session.GetSessionImplementation()?.PersistenceContext;
            if(context is null)
                throw new InvalidOperationException($@"Cannot unproxy; a compatible implementation of {nameof(ISession)} must be provided.
The expression `session?.{nameof(ISession.GetSessionImplementation)}()?.{nameof(ISessionImplementor.PersistenceContext)}` must not return a null reference.");
            return context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUnproxyingService"/> class from an NHibernate <see cref="ISession"/>.
        /// </summary>
        /// <param name="session">An NHibernate session.</param>
        public ObjectUnproxyingService(ISession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }
    }
}
