using System;

namespace CSF.NHibernate
{
    /// <summary>
    /// An object which can 'unwrap' an NHibernate proxy object and return a
    /// representation of the same object as its original type, without proxying.
    /// </summary>
    public interface IUnproxiesObject
    {
        /// <summary>
        /// 'Unproxy' the specified object and get a representation of the same
        /// object which is not an NHibernate proxy.
        /// </summary>
        /// <returns>A representation of the same <paramref name="obj"/>, which is not an NHibernate proxy.</returns>
        /// <param name="obj">The object to 'unproxy'.</param>
        /// <typeparam name="T">The object type.</typeparam>
        T Unproxy<T>(T obj) where T : class;
    }
}
