using System;
using CSF.NHibernate.Model;
using NUnit.Framework;

namespace CSF.NHibernate5
{
    [TestFixture,NonParallelizable]
    public class Rfc4122BinaryGuidTypeTests
    {
        [Test, AutoMoqData]
        public void Session_can_persist_and_load_entity_which_contains_binary_guid(Guid guid, GuidEntity entity)
        {
            using (var session = SessionProvider.Default.GetSession())
            {
                entity.Guid = guid;
                long id;

                using (var tran = session.BeginTransaction())
                {
                    id = (long) session.Save(entity);
                    tran.Commit();
                }

                session.Evict(entity);
                var retrievedEntity = session.Get<GuidEntity>(id);

                Assert.That(retrievedEntity.Guid, Is.EqualTo(guid));
            }
        }
    }
}
