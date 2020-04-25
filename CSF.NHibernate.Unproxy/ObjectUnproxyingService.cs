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
        readonly IStatelessSession statelessSession;

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

            var impl = GetSessionImplementor();
            return (impl?.PersistenceContext is null)? obj : (T)impl.PersistenceContext.Unproxy(obj);
        }

        ISessionImplementor GetSessionImplementor()
            => session?.GetSessionImplementation() ?? statelessSession?.GetSessionImplementation();

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUnproxyingService"/> class from an NHibernate <see cref="ISession"/>.
        /// </summary>
        /// <param name="session">An NHibernate session.</param>
        public ObjectUnproxyingService(ISession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectUnproxyingService"/> class from an NHibernate <see cref="IStatelessSession"/>.
        /// </summary>
        /// <param name="session">An NHibernate stateless session.</param>
        public ObjectUnproxyingService(IStatelessSession session)
        {
            statelessSession = session ?? throw new ArgumentNullException(nameof(session));
        }
    }
}
