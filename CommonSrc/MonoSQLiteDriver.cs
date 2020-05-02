using NHibernate.Driver;

namespace CSF.NHibernate
{
    /// <summary>
    /// <para>
    /// A <see cref="ReflectionBasedDriver"/> for the SQLite database, which uses
    /// Mono's built-in SQLite ADO driver.  This works around an issue whereby
    /// NHibernate's stock <see cref="SQLite20Driver"/> crashes on Mono because it
    /// uses a non-compatible ADO driver.
    /// </para>
    /// <para>
    /// Ideally, do not use this driver directly.  Instead, use <seealso cref="MonoSafeSQLite20Driver"/>,
    /// which will internally use this driver when executing on the Mono runtime.
    /// If not using the Mono runtime, the Mono-safe SQLite driver will use the
    /// NHibernate stock <see cref="SQLite20Driver"/>.  This means that the appropriate
    /// driver will automatically be selected for the environment.
    /// </para>
    /// </summary>
    public class MonoSQLiteDriver : ReflectionBasedDriver
    {
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
        /// <param name="assemblyFinder">A service which provides the assembly which contains the Mono SQLite ADO driver.</param>
        public MonoSQLiteDriver(IGetsMonoSQLiteAssembly assemblyFinder)
            : base(DriverNamespace, assemblyFinder.GetMonoSQLiteAssembly().FullName, ConnectionQualifiedTypeName, CommandQualifiedTypeName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSQLiteDriver"/> class.
        /// </summary>
        public MonoSQLiteDriver() : this(new MonoSQLiteAssemblyFinder()) { }
    }
}

