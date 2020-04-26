using System;
namespace CSF.NHibernate.Model
{
    public class FractionEntity
    {
        public virtual long Id { get; set; }

        public virtual Fraction Fraction { get; set; }

        public virtual string Name { get; set; }
    }
}
