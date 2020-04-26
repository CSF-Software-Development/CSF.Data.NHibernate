using System;
using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace CSF.NHibernate
{
    public class SchemaCreator
    {
        public virtual void CreateSchema(Configuration config, IDbConnection connection)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            var exporter = new SchemaExport(config);
            exporter.Execute(false, true, false, connection, null);
        }
    }
}
