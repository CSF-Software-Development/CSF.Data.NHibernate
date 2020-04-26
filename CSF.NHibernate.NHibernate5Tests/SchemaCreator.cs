using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace CSF.NHibernate
{
    public class SchemaCreator
    {
        public virtual void CreateSchema(Configuration config, DbConnection connection)
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
