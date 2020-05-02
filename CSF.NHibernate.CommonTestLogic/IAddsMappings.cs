using System;
using NHibernate.Cfg;

namespace CSF.NHibernate
{
    public interface IAddsMappings
    {
        void AddMappings(Configuration config);
    }
}
