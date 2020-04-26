using System;
using NHibernate.Cfg;

namespace CSF.NHibernate
{
    public interface IAddsDbDriver
    {
        void AddDriver(Configuration config);
    }
}
