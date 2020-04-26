using System;
using NHibernate.Cfg;
using NHibernate.Dialect;
using static NHibernate.Cfg.Environment;

namespace CSF.NHibernate
{
    public class NHibernateConfigurationProvider
    {
        readonly IAddsMappings mappingProvider;
        readonly IAddsDbDriver driverProvider;

        public virtual Configuration GetConfiguration()
        {
            var config = new Configuration();

            driverProvider.AddDriver(config);
            mappingProvider.AddMappings(config);

            return config;
        }

        public NHibernateConfigurationProvider(IAddsMappings mappingProvider, IAddsDbDriver driverProvider)
        {
            this.mappingProvider = mappingProvider ?? throw new ArgumentNullException(nameof(mappingProvider));
            this.driverProvider = driverProvider ?? throw new ArgumentNullException(nameof(driverProvider));
        }
    }
}
