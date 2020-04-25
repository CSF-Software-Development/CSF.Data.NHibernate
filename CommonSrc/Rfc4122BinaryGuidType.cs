using System;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;
using System.Data;
using NHibernate;
#if NHIBERNATE5
using System.Data.Common;
using NHibernate.Engine;
#endif

namespace CSF.Data.NHibernate
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
        /// Determines whether two instances of this type are equal or not.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public new bool Equals(object x, object y) => Equals(x, y);

        /// <summary>
        /// Get a hashcode for the instance, consistent with persistence "equality"
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="x">The x coordinate.</param>
        public int GetHashCode(object x) => x.GetHashCode();


#if NHIBERNATE4

        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO resultset.
        /// Implementors should handle possibility of null values.
        /// </summary>
        /// <param name="rs">a IDataReader</param>
        /// <param name="names">column names</param>
        /// <param name="owner">the containing entity</param>
        /// <returns></returns>
        /// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var bytes = (byte[])NHibernateUtil.Binary.NullSafeGet(rs, names[0]);

            if (bytes?.Length != GuidByteCount) return null;

            return bytes.ToRFC4122Guid();
        }

        /// <summary>
        /// Write an instance of the mapped class to a prepared statement.
        ///  Implementors should handle possibility of null values.
        ///  A multi-column type should be written to parameters starting from index.
        /// </summary>
        /// <param name="cmd">a IDbCommand</param>
        /// <param name="value">the object to write</param>
        /// <param name="index">command parameter index</param>
        /// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (!(value is Guid guid))
            {
                NHibernateUtil.Binary.NullSafeSet(cmd, null, index);
                return;
            }

            var bytes = guid.ToRFC4122ByteArray();
            NHibernateUtil.Binary.NullSafeSet(cmd, bytes, index);
        }

#elif NHIBERNATE5

        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO resultset.
        /// Implementors should handle possibility of null values.
        /// </summary>
        /// <param name="rs">a IDataReader</param>
        /// <param name="names">column names</param>
        /// <param name="owner">the containing entity</param>
        /// <returns></returns>
        /// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var bytes = (byte[]) NHibernateUtil.Binary.NullSafeGet(rs, names[0], session);

            if (bytes?.Length != GuidByteCount) return null;

            return bytes.ToRFC4122Guid();
        }

        /// <summary>
        /// Write an instance of the mapped class to a prepared statement.
        ///  Implementors should handle possibility of null values.
        ///  A multi-column type should be written to parameters starting from index.
        /// </summary>
        /// <param name="cmd">a IDbCommand</param>
        /// <param name="value">the object to write</param>
        /// <param name="index">command parameter index</param>
        /// <exception cref="T:NHibernate.HibernateException">HibernateException</exception>
        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if(!(value is Guid guid))
            {
                NHibernateUtil.Binary.NullSafeSet(cmd, null, index, session);
                return;
            }

            var bytes = guid.ToRFC4122ByteArray();
            NHibernateUtil.Binary.NullSafeSet(cmd, bytes, index, session);
        }

#endif

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

