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
    public class MonoSafeSQLite20Driver : IDriver, ISqlParameterFormatter
    {
        static readonly IDetectsMono monoDetector = new MonoRuntimeDetector();
        readonly ReflectionBasedDriver wrappedDriver;

        public bool SupportsMultipleOpenReaders
            => wrappedDriver.SupportsMultipleOpenReaders;

        public bool SupportsMultipleQueries
            => wrappedDriver.SupportsMultipleQueries;

        public void AdjustCommand(IDbCommand command)
            => wrappedDriver.AdjustCommand(command);

        public void Configure(IDictionary<string, string> settings)
            => wrappedDriver.Configure(settings);

        public IDbConnection CreateConnection()
            => wrappedDriver.CreateConnection();

        public void ExpandQueryParameters(IDbCommand cmd, SqlString sqlString)
            => wrappedDriver.ExpandQueryParameters(cmd, sqlString);

        public IDbCommand GenerateCommand(CommandType type, SqlString sqlString, SqlType[] parameterTypes)
            => wrappedDriver.GenerateCommand(type, sqlString, parameterTypes);

        public IDbDataParameter GenerateParameter(IDbCommand command, string name, SqlType sqlType)
            => wrappedDriver.GenerateParameter(command, name, sqlType);

        public string GetParameterName(int index)
            => GetParameterName(index);

        public IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
            => wrappedDriver.GetResultSetsCommand(session);

        public void PrepareCommand(IDbCommand command)
            => wrappedDriver.PrepareCommand(command);

        public void RemoveUnusedCommandParameters(IDbCommand cmd, SqlString sqlString)
            => wrappedDriver.RemoveUnusedCommandParameters(cmd, sqlString);

        public MonoSafeSQLite20Driver()
        {
            if (monoDetector.IsExecutingWithMono())
                wrappedDriver = new MonoSQLiteDriver();
            else
                wrappedDriver = new SQLite20Driver();
        }
    }
}
