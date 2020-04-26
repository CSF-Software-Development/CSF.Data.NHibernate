using System;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;
using System.Data;
using NHibernate;
#if NHIBERNATE5
using System.Data.Common;
using NHibernate.Engine;
#endif

namespace CSF.NHibernate
{
    /// <summary>
    /// An NHibernate <see cref="IUserType"/> which stores a <see cref="Guid"/> as binary
    /// data, in RFC-4122 format.
    /// </summary>
    public class Rfc4122BinaryGuidType : IUserType
    {
        const int GuidByteCount = 16;
        static readonly SqlType[] Types = { new SqlType(DbType.Binary) };

        /// <summary>
        /// Gets a collection of the SQL column types used to store the value.
        /// </summary>
        /// <value>The sql types.</value>
        public SqlType[] SqlTypes => Types;

        /// <summary>
        /// Gets the returned object type.
        /// </summary>
        /// <value>The type of the returned.</value>
        public Type ReturnedType => typeof(Guid);

        /// <summary>
        /// Determines whether two instances of this type are equal or not, for the purposes of persistence.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        public new bool Equals(object x, object y) => Object.Equals(x, y);

        /// <summary>
        /// Get a hashcode for the instance, consistent with persistence "equality"
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="x">The object.</param>
        public int GetHashCode(object x) => x.GetHashCode();

#if NHIBERNATE4
        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO resultset.
        /// </summary>
        /// <param name="rs">An ADO data-reader object</param>
        /// <param name="names">An array of the column names from which to get data</param>
        /// <param name="owner">The containing entity</param>
        /// <returns>A <see cref="Guid"/>, or a null reference.</returns>
        /// <exception cref="HibernateException"></exception>
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
#elif NHIBERNATE5
        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO resultset.
        /// </summary>
        /// <param name="rs">An ADO data-reader object</param>
        /// <param name="names">An array of the column names from which to get data</param>
        /// <param name="owner">The containing entity</param>
        /// <param name="session">A session implementor</param>
        /// <returns>A <see cref="Guid"/>, or a null reference.</returns>
        /// <exception cref="HibernateException"></exception>
        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
#endif
        {
#if NHIBERNATE4
            var bytes = (byte[])NHibernateUtil.Binary.NullSafeGet(rs, names[0]);
#elif NHIBERNATE5
            var bytes = (byte[]) NHibernateUtil.Binary.NullSafeGet(rs, names[0], session);
#endif

            if (bytes?.Length != GuidByteCount) return null;
            return bytes.ToRFC4122Guid();
        }

#if NHIBERNATE4
        /// <summary>
        /// Write an instance of the mapped class to a prepared statement.
        /// </summary>
        /// <param name="cmd">An ADO database command object</param>
        /// <param name="value">The object to write</param>
        /// <param name="index">The zero-based index of the column (used for multi-column writes)</param>
        /// <exception cref="HibernateException"></exception>
        public void NullSafeSet(IDbCommand cmd, object value, int index)
#elif NHIBERNATE5
        /// <summary>
        /// Write an instance of the mapped class to a prepared statement.
        /// </summary>
        /// <param name="cmd">An ADO database command object</param>
        /// <param name="value">The object to write</param>
        /// <param name="index">The zero-based index of the column (used for multi-column writes)</param>
        /// <param name="session">A session implementor</param>
        /// <exception cref="HibernateException"></exception>
        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
#endif
        {
            if (!(value is Guid guid))
            {
#if NHIBERNATE4
                NHibernateUtil.Binary.NullSafeSet(cmd, null, index);
#elif NHIBERNATE5
                NHibernateUtil.Binary.NullSafeSet(cmd, null, index, session);
#endif
                return;
            }

            var bytes = guid.ToRFC4122ByteArray();
#if NHIBERNATE4
            NHibernateUtil.Binary.NullSafeSet(cmd, bytes, index);
#elif NHIBERNATE5
            NHibernateUtil.Binary.NullSafeSet(cmd, bytes, index, session);
#endif
        }

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        /// <param name="value">generally a collection element or entity field</param>
        /// <returns>a copy</returns>
        public object DeepCopy(object value)
        {
            return value;
        }

        /// <summary>
        /// Are objects of this type mutable?
        /// </summary>
        /// <value><c>true</c> if this instance is mutable; otherwise, <c>false</c>.</value>
        public bool IsMutable => false;

        /// <summary>
        /// Replace the specified original, target and owner.
        /// </summary>
        /// <param name="original">Original.</param>
        /// <param name="target">Target.</param>
        /// <param name="owner">Owner.</param>
        public object Replace(object original, object target, object owner) => original;

        /// <summary>
        /// Reconstruct an object from the cacheable representation. At the very least this
        ///  method should perform a deep copy if the type is mutable. (optional operation)
        /// </summary>
        /// <param name="cached">the object to be cached</param>
        /// <param name="owner">the owner of the cached object</param>
        /// <returns>a reconstructed object from the cachable representation</returns>
        public object Assemble(object cached, object owner) => cached;

        /// <summary>
        /// Transform the object into its cacheable representation. At the very least this
        ///  method should perform a deep copy if the type is mutable. That may not be enough
        ///  for some implementations, however; for example, associations must be cached as
        ///  identifier values. (optional operation)
        /// </summary>
        /// <param name="value">the object to be cached</param>
        /// <returns>a cacheable representation of the object</returns>
        public object Disassemble(object value) => value;
    }
}

