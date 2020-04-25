using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace CSF.NHibernate.CommonTestLogic
{
    public class SchemaCreator
    {
        public virtual void CreateSchema(Configuration config, ISessionFactory sessionFactory)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (sessionFactory == null)
                throw new ArgumentNullException(nameof(sessionFactory));

            using (var session = sessionFactory.OpenSession())
            {
                var connection = session.Connection;
                var exporter = new SchemaExport(config);

                exporter.Execute(false, true, false, connection, null);
            }
        }
    }
}
