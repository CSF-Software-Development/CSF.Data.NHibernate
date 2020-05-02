using System;
using CSF.NHibernate.Model;
using NUnit.Framework;

namespace CSF.NHibernate
{
    [TestFixture,NonParallelizable]
    public class FractionTypeTests
    {
        [Test, AutoMoqData]
        public void Session_can_persist_and_load_entity_which_contains_fraction(FractionEntity entity, Fraction fraction)
        {
            using (var session = SessionProvider.Default.GetSession())
            {
                entity.Fraction = fraction;
                long id;

                using (var tran = session.BeginTransaction())
                {
                    id = (long) session.Save(entity);
                    tran.Commit();
                }

                session.Evict(entity);
                var retrievedEntity = session.Get<FractionEntity>(id);

                Assert.That(retrievedEntity.Fraction, Is.EqualTo(fraction));
            }
        }
    }
}
