using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using static NHibernate.Cfg.Environment;

namespace CSF.NHibernate
{
    public class NHibernateConfigurationProvider
    {
        readonly IAddsMappings mappingProvider;

        public virtual Configuration GetConfiguration()
        {
            var config = new Configuration();

            AddDbDriver(config);
            mappingProvider.AddMappings(config);
            return config;
        }

        protected virtual void AddDbDriver(Configuration config)
        {
            config.DataBaseIntegration(x => {
                x.Driver<MonoSafeSQLite20Driver>();
                x.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                x.Dialect<SQLiteDialect>();
            });
            config.SetProperty(ReleaseConnections, "on_close");
        }

        public NHibernateConfigurationProvider(IAddsMappings mappingProvider)
        {
            this.mappingProvider = mappingProvider ?? throw new ArgumentNullException(nameof(mappingProvider));
        }
    }
}
