#region license
// Copyright (c) 2020 rubicon IT GmbH, www.rubicon.eu
// Copyright (c) 2005 - 2009 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

#if NETFRAMEWORK

using System;
using System.Web.Security;
using NUnit.Framework;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_StevenS : IDisposable
	{
		private MockRepository mocks;
		private MembershipProvider myMembershipProvider;

        [SetUp]
		public void SetUp()
		{
			mocks = new MockRepository();
			myMembershipProvider = mocks.StrictMock<MembershipProvider>();
		} 

        [Test]
        public void LoadFromUserId()
        {
            SetupResult.For(myMembershipProvider.Name).Return("Foo");

            Expect.Call(myMembershipProvider.GetUser("foo",false)).Return(null);

            mocks.ReplayAll();

        	myMembershipProvider.GetUser("foo", false);
        }

		[Test]
		public void LoadFromUserId_Object()
		{
			SetupResult.For(myMembershipProvider.Name).Return("Foo");

			object foo = "foo";
			Expect.Call(myMembershipProvider.GetUser(foo, false)).Return(null);

			mocks.ReplayAll();

			myMembershipProvider.GetUser(foo, false);
		} 


		public void Dispose()
		{
			mocks.VerifyAll();
		}
	}
}

#endif
