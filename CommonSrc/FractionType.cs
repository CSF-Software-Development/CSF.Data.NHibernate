using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
#if NHIBERNATE5
using System.Data.Common;
using NHibernate.Engine;
#endif

namespace CSF.NHibernate
{
    /// <summary>
    /// An NHibernate <see cref="IUserType"/> for storing a <see cref="Fraction"/> instance in a database.
    /// </summary>
    public class FractionType : IUserType
    {
        static readonly SqlType[] Types = {
            new SqlType(DbType.Int64),
            new SqlType(DbType.Int64),
            new SqlType(DbType.Int64),
            new SqlType(DbType.Boolean),
        };

        /// <summary>
        /// Gets a collection of the SQL column types used to store the value.
        /// </summary>
        /// <value>The sql types.</value>
        public SqlType[] SqlTypes => Types;

        /// <summary>
        /// Gets the returned object type.
        /// </summary>
        /// <value>The type of the returned.</value>
        public Type ReturnedType => typeof(Fraction);

        /// <summary>
        /// Determines whether two instances of this type are equal or not.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public new bool Equals(object x, object y) => Object.Equals(x, y);

        /// <summary>
        /// Get a hashcode for the instance, consistent with persistence "equality"
        /// </summary>
        /// <returns>The hash code.</returns>
        /// <param name="x">The x coordinate.</param>
        public int GetHashCode(object x) => (x?.GetHashCode()).GetValueOrDefault();


#if NHIBERNATE4
        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO resultset.
        /// </summary>
        /// <param name="rs">An ADO data-reader object</param>
        /// <param name="names">An array of the column names from which to get data</param>
        /// <param name="owner">The containing entity</param>
        /// <returns>A <see cref="Fraction"/>, or a null reference.</returns>
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
        /// <returns>A <see cref="Fraction"/>, or a null reference.</returns>
        /// <exception cref="HibernateException"></exception>
        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
#endif
        {
#if NHIBERNATE4
            long?
                integer = (long?)NHibernateUtil.Int64.NullSafeGet(rs, names[0]),
                numerator = (long?)NHibernateUtil.Int64.NullSafeGet(rs, names[1]),
                denominator = (long?)NHibernateUtil.Int64.NullSafeGet(rs, names[2]); 
            bool? isNegative = (bool?)NHibernateUtil.Boolean.NullSafeGet(rs, names[3]);
#elif NHIBERNATE5
            long?
                integer = (long?)NHibernateUtil.Int64.NullSafeGet(rs, names[0], session),
                numerator = (long?)NHibernateUtil.Int64.NullSafeGet(rs, names[1], session),
                denominator = (long?)NHibernateUtil.Int64.NullSafeGet(rs, names[2], session); 
            bool? isNegative = (bool?)NHibernateUtil.Boolean.NullSafeGet(rs, names[3], session);
#endif

            if (!numerator.HasValue || !denominator.HasValue || !integer.HasValue || !isNegative.HasValue)
                return null;

            return new Fraction(integer.Value, numerator.Value, denominator.Value, isNegative.Value);
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
            if (!(value is Fraction fraction))
            {
#if NHIBERNATE4
                NHibernateUtil.Int64.NullSafeSet(cmd, null, index);
#elif NHIBERNATE5
                NHibernateUtil.Int64.NullSafeSet(cmd, null, index, session);
#endif
                return;
            }

#if NHIBERNATE4
            if (index == 0) NHibernateUtil.Int64.NullSafeSet(cmd, fraction.AbsoluteInteger, index);
            if (index == 1) NHibernateUtil.Int64.NullSafeSet(cmd, fraction.Numerator, index);
            if (index == 2) NHibernateUtil.Int64.NullSafeSet(cmd, fraction.Denominator, index);
            if (index == 3) NHibernateUtil.Boolean.NullSafeSet(cmd, fraction.IsNegative, index);
#elif NHIBERNATE5
            if (index == 0) NHibernateUtil.Int64.NullSafeSet(cmd, fraction.AbsoluteInteger, index, session);
            if (index == 1) NHibernateUtil.Int64.NullSafeSet(cmd, fraction.Numerator, index, session);
            if (index == 2) NHibernateUtil.Int64.NullSafeSet(cmd, fraction.Denominator, index, session);
            if (index == 3) NHibernateUtil.Boolean.NullSafeSet(cmd, fraction.IsNegative, index, session);
#endif
        }

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        /// <param name="value">generally a collection element or entity field</param>
        /// <returns>a copy</returns>
        public object DeepCopy(object value) => value;

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

