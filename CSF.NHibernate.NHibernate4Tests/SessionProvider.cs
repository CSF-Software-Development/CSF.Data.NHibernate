using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace CSF.NHibernate
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

        public IStatelessSession GetStatelessSession()
        {
            var session = sessionFactory.OpenStatelessSession();
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

        public static SessionProvider Default { get; } = new SessionProvider();
    }
}
