using System;
using NHibernate.Driver;
using System.Reflection;

namespace CSF.NHibernate
{
    /// <summary>
    /// A <see cref="ReflectionBasedDriver"/> for the SQLite database, which makes use of the
    /// Mono-built-in SQLite driver (found in the <c>Mono.Data.Sqlite</c> namespace).  This works
    /// around issues whereby <see cref="SQLite20Driver"/>, which ships with NHibernate does not work
    /// on the Mono runtime.
    public class MonoSQLiteDriver : ReflectionBasedDriver
    {
        static readonly IGetsMonoSQLiteAssembly AssemblyFinder = new MonoSQLiteAssemblyFinder();

        static readonly string
          DriverNamespace = "Mono.Data.Sqlite",
          ConnectionQualifiedTypeName = $"{DriverNamespace}.SqliteConnection",
          CommandQualifiedTypeName = $"{DriverNamespace}.SqliteCommand";

        /// <summary>
        /// Gets a value indicating whether this <see cref="MonoSQLiteDriver"/> uses named
        /// prefixes in parameters.
        /// </summary>
        /// <value>
        /// <c>true</c> if this driver uses named prefix in parameters; otherwise, <c>false</c>.
        /// </value>
        public override bool UseNamedPrefixInParameter => true;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MonoSQLiteDriver"/> uses named
        /// prefixes in sql.
        /// </summary>
        /// <value>
        /// <c>true</c> if this driver uses named prefixes in sql; otherwise, <c>false</c>.
        /// </value>
        public override bool UseNamedPrefixInSql => true;

        /// <summary>
        /// Gets the named parameter prefix.
        /// </summary>
        /// <value>
        /// The named parameter prefix.
        /// </value>
        public override string NamedPrefix => "@";

        /// <summary>
        /// Gets a value indicating whether this <see cref="MonoSQLiteDriver"/> supports
        /// multiple open readers.
        /// </summary>
        /// <value>
        /// <c>true</c> if this driver supports multiple open readers; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultipleOpenReaders => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSQLiteDriver"/> class.
        /// </summary>
        public MonoSQLiteDriver() : this(AssemblyFinder.GetMonoSQLiteAssembly()) { }

        public MonoSQLiteDriver(Assembly assembly)
            : base(DriverNamespace, assembly.FullName, ConnectionQualifiedTypeName, CommandQualifiedTypeName) { }
    }
}

