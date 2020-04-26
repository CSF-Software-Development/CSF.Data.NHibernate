using System;
using CSF.NHibernate;
using CSF.NHibernate.Model;
using NHibernate.Mapping.ByCode.Conformist;

namespace CSF.NHibernate4.Mappings
{
    public class GuidEntityMap : ClassMapping<GuidEntity>
    {
        public GuidEntityMap()
        {
            Id(x => x.Id);

            Property(x => x.Guid, m => m.Type<Rfc4122BinaryGuidType>());
        }
    }
}
