using System;
using System.Collections;
using System.Reflection;
using CSF.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NHibernate.Type;
using static NHibernate.Cfg.Environment;

namespace CSF.NHibernate4
{
    public class SessionProvider
    {
        readonly Configuration config;
        readonly ISessionFactory sessionFactory;

        public ISession GetSession()
        {
            var session = sessionFactory.OpenSession();
            var schemaCreator = new SchemaCreator();
            schemaCreator.CreateSchema(config, session.Connection);

            return session;
        }

        private SessionProvider()
        {
            var mappingProvider = new AssemblyMappingsProvider(Assembly.GetExecutingAssembly());
            var driverProvider = new DriverProvider();
            var configProvider = new NHibernateConfigurationProvider(mappingProvider, driverProvider);

            config = configProvider.GetConfiguration();
            sessionFactory = config.BuildSessionFactory();
        }

        static SessionProvider defaultInstance;
        public static SessionProvider Default
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = new SessionProvider();
                return defaultInstance;
            }
        }

        class DriverProvider : IAddsDbDriver
        {
            public void AddDriver(Configuration config)
            {
                config.DataBaseIntegration(x => {
                    x.Driver<MonoSafeSQLite20Driver>();
                    x.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                    x.Dialect<SQLiteDialect>();
                });
                config.SetProperty(ReleaseConnections, "on_close");
            }
        }
    }
}
