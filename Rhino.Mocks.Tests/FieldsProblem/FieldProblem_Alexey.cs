#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Text;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System.Data.SqlClient;
	using Exceptions;
	using NUnit.Framework;

	public interface ITestInterface
	{
		ITestInterface AddService<TService, TComponent>() where TComponent : TService;
	}

	public class TestInterface : MarshalByRefObject
	{
		public virtual TestInterface AddService<TService, TComponent>() where TComponent : TService
		{
			return this;
		}
	}

	
	public class FieldProblem_Alexey
	{
		[Test]
		public void MockInterfaceWithGenericMethodWithConstraints()
		{
			MockRepository mockery = new MockRepository();

			ITestInterface mockObj = mockery.StrictMockWithRemoting<ITestInterface>();
			mockObj.AddService<IDisposable, SqlConnection>();
			LastCall.Return(mockObj);
			mockery.ReplayAll();

			mockObj.AddService<IDisposable, SqlConnection>();

			mockery.VerifyAll();
		}

		[Test]
		public void MockInterfaceWithGenericMethodWithConstraints_WhenNotValid()
		{
			MockRepository mockery = new MockRepository();

			ITestInterface mockObj = mockery.StrictMockWithRemoting<ITestInterface>();
			mockObj.AddService<IDisposable, SqlConnection>();
			LastCall.Return(mockObj);
			mockery.ReplayAll();

            Assert.Throws<ExpectationViolationException> (
                () => mockery.VerifyAll(),
                "ITestInterface.AddService<System.IDisposable, System.Data.SqlClient.SqlConnection>(); Expected #1, Actual #0.");
		}

		[Test]
		public void MockInterfaceWithGenericMethodWithConstraints_WhenNotValid_UsingDynamicMock()
		{
			MockRepository mockery = new MockRepository();

			ITestInterface mockObj = mockery.DynamicMockWithRemoting<ITestInterface>();
			mockObj.AddService<IDisposable, SqlConnection>();
			LastCall.Return(mockObj);
			mockery.ReplayAll();

            Assert.Throws<ExpectationViolationException> (
                () => mockery.VerifyAll(),
                "ITestInterface.AddService<System.IDisposable, System.Data.SqlClient.SqlConnection>(); Expected #1, Actual #0.");
		}

		[Test]
		public void MockInterfaceWithGenericMethodWithConstraints_UsingDynamicMock()
		{
			MockRepository mockery = new MockRepository();

			ITestInterface mockObj = mockery.DynamicMockWithRemoting<ITestInterface>();
			mockery.ReplayAll();

			mockObj.AddService<IDisposable, SqlConnection>();

			mockery.VerifyAll();
		}
	}
}

#endif
