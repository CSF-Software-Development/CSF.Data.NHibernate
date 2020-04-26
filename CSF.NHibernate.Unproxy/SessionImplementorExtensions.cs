using NHibernate;

namespace CSF.NHibernate
{
    /// <summary>
    /// Convenience extension methods for unproxying objects.
    /// </summary>
    public static class SessionImplementorExtensions
    {
        /// <summary>
        /// Unproxy the specified object.  See <seealso cref="ObjectUnproxyingService"/> for
        /// a description of the functionality.  This method is a convenient way of executing
        /// the functionality from that service.
        /// </summary>
        /// <returns>A representation of the same <paramref name="obj"/>, which is not an NHibernate proxy.</returns>
        /// <param name="session">An NHibernate <see cref="ISession"/>.</param>
        /// <param name="obj">The object to 'unproxy'.</param>
        /// <typeparam name="T">The object type.</typeparam>
        public static T Unproxy<T>(this ISession session, T obj) where T : class
        {
            return new ObjectUnproxyingService(session).Unproxy(obj);
        }
    }
}
