using System;
using System.Reflection;
using CSF.NHibernate;
using NHibernate;
using NHibernate.Cfg;

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
            var configProvider = new NHibernateConfigurationProvider(mappingProvider);

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
    }
}
