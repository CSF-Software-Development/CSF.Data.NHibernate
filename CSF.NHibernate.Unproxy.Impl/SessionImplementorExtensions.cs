using NHibernate;

namespace CSF.NHibernate
{
    public static class SessionImplementorExtensions
    {
        public static T Unproxy<T>(this ISession session, T obj) where T : class
        {
            return new ObjectUnproxyingService(session).Unproxy(obj);
        }

        public static T Unproxy<T>(this IStatelessSession session, T obj) where T : class
        {
            return new ObjectUnproxyingService(session).Unproxy(obj);
        }
    }
}
