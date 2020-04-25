using System;

namespace CSF.NHibernate
{
    public interface IUnproxiesObject
    {
        T Unproxy<T>(T obj) where T : class;
    }
}
