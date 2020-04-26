using System;
using CSF.NHibernate;
using CSF.NHibernate.Model;
using NHibernate.Mapping.ByCode.Conformist;

namespace CSF.NHibernate4.Mappings
{
    public class FractionEntityMap : ClassMapping<FractionEntity>
    {
        public FractionEntityMap()
        {
            Id(x => x.Id);

            Property(x => x.Fraction, m => {
                m.Type<FractionType>();
                m.Columns(c => c.Name("FractionInteger"),
                          c => c.Name("FractionNumerator"),
                          c => c.Name("FractionDenominator"),
                          c => c.Name("FractionIsNegative"));
            });
        }
    }
}
