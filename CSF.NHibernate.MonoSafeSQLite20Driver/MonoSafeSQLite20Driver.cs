using System;
using System.Collections.Generic;
using System.Data;
using CSF.Reflection;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace CSF.NHibernate
{
    /// <summary>
    /// An NHibernate database driver which wraps either of <see cref="SQLite20Driver"/> or
    /// <see cref="MonoSQLiteDriver"/>, depending upon whether it is executing under the Mono
    /// runtime or not.  This allows a single driver to be used by the consuming application
    /// for SQLite, and it will always use the appropriate implementation for the envioronment.
    /// </summary>
    public class MonoSafeSQLite20Driver : IDriver, ISqlParameterFormatter
    {
        static readonly IDetectsMono monoDetector = new MonoRuntimeDetector();
        readonly ReflectionBasedDriver wrappedDriver;

        /// <summary>
        /// Does this Driver support having more than 1 open IDataReader with
        /// the same IDbConnection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A value of <see langword="false" /> indicates that an exception would be thrown if NHibernate
        /// attempted to have 2 IDataReaders open using the same IDbConnection.  NHibernate
        /// (since this version is a close to straight port of Hibernate) relies on the
        /// ability to recursively open 2 IDataReaders.  If the Driver does not support it
        /// then NHibernate will read the values from the IDataReader into an <see cref="T:NHibernate.Driver.NDataReader" />.
        /// </para>
        /// <para>
        /// A value of <see langword="true" /> will result in greater performance because an IDataReader can be used
        /// instead of the <see cref="T:NHibernate.Driver.NDataReader" />.  So if the Driver supports it then make sure
        /// it is set to <see langword="true" />.
        /// </para>
        /// </remarks>
        public bool SupportsMultipleOpenReaders
            => wrappedDriver.SupportsMultipleOpenReaders;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:CSF.NHibernate.MonoSafeSQLite20Driver"/> supports multiple queries.
        /// </summary>
        /// <value><c>true</c> if supports multiple queries; otherwise, <c>false</c>.</value>
        public bool SupportsMultipleQueries
            => wrappedDriver.SupportsMultipleQueries;

        /// <summary>
        /// Make any adjustments to each IDbCommand object before it is added to the batcher.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <remarks>
        /// This method should be executed before add each single command to the batcher.
        /// If you have to adjust parameters values/type (when the command is full filled) this is a good place where do it.
        /// </remarks>
        public void AdjustCommand(IDbCommand command)
            => wrappedDriver.AdjustCommand(command);

        /// <summary>
        /// Configure the driver using <paramref name="settings" />.
        /// </summary>
        public void Configure(IDictionary<string, string> settings)
            => wrappedDriver.Configure(settings);

        /// <summary>
        /// Creates an uninitialized IDbConnection object for the specific Driver
        /// </summary>
        public IDbConnection CreateConnection()
            => wrappedDriver.CreateConnection();

        /// <summary>
        /// Expand the parameters of the cmd to have a single parameter for each parameter in the
        /// sql string
        /// </summary>
        /// <remarks>
        /// This is for databases that do not support named parameters.  So, instead of a single parameter
        /// for 'select ... from MyTable t where t.Col1 = @p0 and t.Col2 = @p0' we can issue
        /// 'select ... from MyTable t where t.Col1 = ? and t.Col2 = ?'
        /// </remarks>
        public void ExpandQueryParameters(IDbCommand cmd, SqlString sqlString)
            => wrappedDriver.ExpandQueryParameters(cmd, sqlString);

        /// <summary>
        /// Generates an IDbCommand from the SqlString according to the requirements of the DataProvider.
        /// </summary>
        /// <param name="type">The <see cref="T:System.Data.CommandType" /> of the command to generate.</param>
        /// <param name="sqlString">The SqlString that contains the SQL.</param>
        /// <param name="parameterTypes">The types of the parameters to generate for the command.</param>
        /// <returns>An IDbCommand with the CommandText and Parameters fully set.</returns>
        public IDbCommand GenerateCommand(CommandType type, SqlString sqlString, SqlType[] parameterTypes)
            => wrappedDriver.GenerateCommand(type, sqlString, parameterTypes);

        /// <summary>
        /// Generates an IDbDataParameter for the IDbCommand.  It does not add the IDbDataParameter to the IDbCommand's
        /// Parameter collection.
        /// </summary>
        /// <param name="command">The IDbCommand to use to create the IDbDataParameter.</param>
        /// <param name="name">The name to set for IDbDataParameter.Name</param>
        /// <param name="sqlType">The SqlType to set for IDbDataParameter.</param>
        /// <returns>An IDbDataParameter ready to be added to an IDbCommand.</returns>
        public IDbDataParameter GenerateParameter(IDbCommand command, string name, SqlType sqlType)
            => wrappedDriver.GenerateParameter(command, name, sqlType);

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <returns>The parameter name.</returns>
        /// <param name="index">Index.</param>
        public string GetParameterName(int index)
            => GetParameterName(index);

        /// <summary>
        /// Gets the result sets command.
        /// </summary>
        /// <returns>The result sets command.</returns>
        /// <param name="session">Session.</param>
        public IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
            => wrappedDriver.GetResultSetsCommand(session);

        /// <summary>
        /// Prepare the <paramref name="command" /> by calling <see cref="IDbCommand.Prepare" />.
        /// May be a no-op if the driver does not support preparing commands, or for any other reason.
        /// </summary>
        /// <param name="command">The command.</param>
        public void PrepareCommand(IDbCommand command)
            => wrappedDriver.PrepareCommand(command);

        /// <summary>
        /// Remove 'extra' parameters from the IDbCommand
        /// </summary>
        /// <remarks>
        /// We sometimes create more parameters than necessary (see NH-2792 &amp; also comments in SqlStringFormatter.ISqlStringVisitor.Parameter)
        /// </remarks>
        public void RemoveUnusedCommandParameters(IDbCommand cmd, SqlString sqlString)
            => wrappedDriver.RemoveUnusedCommandParameters(cmd, sqlString);

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoSafeSQLite20Driver"/> class.
        /// </summary>
        public MonoSafeSQLite20Driver()
        {
            if (monoDetector.IsExecutingWithMono())
                wrappedDriver = new MonoSQLiteDriver();
            else
                wrappedDriver = new SQLite20Driver();
        }
    }
}
