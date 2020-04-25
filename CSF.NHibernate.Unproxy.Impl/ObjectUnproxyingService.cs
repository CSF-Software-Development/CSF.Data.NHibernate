using System;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Proxy;

namespace CSF.NHibernate
{
    public class ObjectUnproxyingService : IUnproxiesObject
    {
        readonly ISession session;
        readonly IStatelessSession statelessSession;

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

        public ObjectUnproxyingService(ISession session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public ObjectUnproxyingService(IStatelessSession session)
        {
            statelessSession = session ?? throw new ArgumentNullException(nameof(session));
        }
    }
}
