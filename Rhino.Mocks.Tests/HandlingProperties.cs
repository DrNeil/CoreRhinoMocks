﻿#region license
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

using System;
using System.Text;
using NUnit.Framework;

namespace Rhino.Mocks.Tests
{
    
    public class HandlingProperties
    {
        IDemo demo;
        MockRepository mocks;

        [SetUp]
		public void SetUp()
        {
            mocks = new MockRepository();
            demo = (IDemo)mocks.StrictMock(typeof(IDemo));
        }

        [Test]
        public void PropertyBehaviorForSingleProperty()
        {
            Expect.Call(demo.Prop).PropertyBehavior();
            mocks.ReplayAll();
            for (int i = 0; i < 49; i++)
            {
                demo.Prop = "ayende" + i;
                Assert.AreEqual("ayende" + i, demo.Prop);
            }
            mocks.VerifyAll();
        }

        [Test]
        public void ExceptionIfLastMethodCallIsNotProperty()
        {
            Assert.Throws<InvalidOperationException> (
                () => Expect.Call (demo.EnumNoArgs()).PropertyBehavior(),
                "Last method call was not made on a setter or a getter");
        }

        [Test]
        public void ExceptionIfPropHasOnlyGetter()
        {
            Assert.Throws<InvalidOperationException> (
                () => Expect.Call (demo.ReadOnly).PropertyBehavior(),
                "Property must be read/write");
        }

        [Test]
        public void ExceptionIfPropHasOnlySetter()
        {
            Assert.Throws<InvalidOperationException> (
                () => Expect.Call (demo.WriteOnly).PropertyBehavior(),
                "Property must be read/write");
        }

        [Test]
        public void IndexedPropertiesSupported()
        {
            IWithIndexers x = (IWithIndexers)mocks.StrictMock(typeof(IWithIndexers));
            Expect.Call(x[1]).PropertyBehavior();
            Expect.Call(x["",1]).PropertyBehavior();
            mocks.ReplayAll();

            x[1] = 10;
            x[10] = 100;
            Assert.AreEqual(10, x[1]);
            Assert.AreEqual(100, x[10]);

            x["1", 2] = "3";
            x["2", 3] = "5";
            Assert.AreEqual("3", x["1", 2]);
            Assert.AreEqual("5", x["2", 3]);

            mocks.VerifyAll();
        }

        [Test]
        public void IndexPropertyWhenValueTypeAndNotFoundThrows()
        {
            IWithIndexers x = (IWithIndexers)mocks.StrictMock(typeof(IWithIndexers));
            Expect.Call(x[1]).PropertyBehavior();
            mocks.ReplayAll();
            Assert.Throws<InvalidOperationException> (
                () => GC.KeepAlive (x[1]),
                "Can't return a value for property Item because no value was set and the Property return a value type.");
        }

        [Test]
        public void IndexPropertyWhenRefTypeAndNotFoundReturnNull()
        {
            IWithIndexers x = (IWithIndexers)mocks.StrictMock(typeof(IWithIndexers));
            Expect.Call(x["",3]).PropertyBehavior();
            mocks.ReplayAll();
            Assert.Null(x["", 2]);
        }

        public interface IWithIndexers
        {
            int this[int x] { get; set; }

            string this[string n, int y] { get; set; } 
        }
    }
}
