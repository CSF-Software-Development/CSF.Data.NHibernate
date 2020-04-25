using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace CSF.NHibernate.Tests
{
    public class NHibernateConfigurationProvider
    {
        public virtual Configuration GetConfiguration()
        {
            var config = new Configuration();

            AddDbDriver(config);
            AddMappings(config);
        }

        protected virtual void AddDbDriver(Configuration config)
        {
            config.DataBaseIntegration(x => {
                x.Driver<MonoSafeSQLite20Driver>();
                x.ConnectionString = "Data Source=:memory:;Version=3;New=True;";
                x.Dialect<SQLiteDialect>();
            });
            config.SetProperty(env.ReleaseConnections, "on_close");
        }

        protected virtual void AddMappings(Configuration config)
        {
            throw new NotImplementedException();
        }
    }
}
