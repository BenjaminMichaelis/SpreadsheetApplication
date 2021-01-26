// <copyright file="TestClass.cs" company="Benjamin Michaelis">
// </copyright>

namespace HW1_ConsoleApp.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            var answer = 42;
            Assert.That(answer, Is.EqualTo(42), "Some useful error message");
        }
    }
}
