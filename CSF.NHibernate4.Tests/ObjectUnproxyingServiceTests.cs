using System;
using CSF.NHibernate;
using CSF.NHibernate.Model;
using NUnit.Framework;

namespace CSF.NHibernate
{
    [TestFixture,NonParallelizable]
    public class ObjectUnproxyingServiceTests
    {
        [Test, AutoMoqData]
        public void Unproxy_gets_correct_object_instance_using_session(FractionEntity entity)
        {
            using (var session = SessionProvider.Default.GetSession())
            {
                long id;

                using (var tran = session.BeginTransaction())
                {
                    id = (long)session.Save(entity);
                    tran.Commit();
                }
                session.Evict(entity);

                var loaded = session.Load<FractionEntity>(id);
                var sut = new ObjectUnproxyingService(session);
                var unproxied = sut.Unproxy(loaded);

                Assert.That(unproxied.GetType(), Is.EqualTo(typeof(FractionEntity)), "Type is correct");
                Assert.That(unproxied.Name, Is.EqualTo(entity.Name), "Name is correct");
            }
        }
    }
}
