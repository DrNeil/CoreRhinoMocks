using System;
using NUnit.Framework;
using Rhino.Mocks.Constraints;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    
    public class FieldProblem_Roy
    {
        [Test]
        public void StubNeverFailsTheTest()
        {
            MockRepository repository = new MockRepository();
            IGetResults resultGetter = repository.Stub<IGetResults>();
            using (repository.Record())
            {
                resultGetter.GetSomeNumber("a");
                LastCall.Return(1);
            }

            int result = resultGetter.GetSomeNumber("b");
            Assert.AreEqual(0, result);
            repository.VerifyAll(); //<- should not fail the test methinks
        }

        [Test]
        public void CanGetSetupResultFromStub()
        {
            MockRepository repository = new MockRepository();
            IGetResults resultGetter = repository.Stub<IGetResults>();
            using (repository.Record())
            {
                resultGetter.GetSomeNumber("a");
                LastCall.Return(1);
            }

            int result = resultGetter.GetSomeNumber("a");
            Assert.AreEqual(1, result);
            repository.VerifyAll(); 
        }

        [Test]
        public void CannotCallLastCallConstraintsMoreThanOnce()
        {
            MockRepository repository = new MockRepository();
            IGetResults resultGetter = repository.Stub<IGetResults>();
            Assert.Throws<InvalidOperationException> (
                () =>
                {
                    using (repository.Record())
                    {
                        resultGetter.GetSomeNumber ("a");
                        LastCall.Constraints (Text.Contains ("b"));
                        LastCall.Constraints (Text.Contains ("a"));
                    }
                },
                "You have already specified constraints for this method. (IGetResults.GetSomeNumber(contains \"b\");)");
        }
    }

    public interface IGetResults
    {
        int GetSomeNumber(string s);
    }
}