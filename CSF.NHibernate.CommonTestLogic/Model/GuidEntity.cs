using System;
namespace CSF.NHibernate.Model
{
    public class GuidEntity
    {
        public virtual long Id { get; set; }

        public virtual Guid Guid { get; set; }
    }
}
