using System;
using ITI.Clavardons.Libraries;
using ITI.Clavardons.Providers;
using NUnit.Framework;
using Pose;

namespace Tests
{
    public class FakeTimeProvider : ITimeProvider
    {
        public DateTime date { get; set; } = DateTime.Now;

        public DateTime GetNow()
        {
            return date;
        }
    }

    public class TestAntiSpam
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void T1_CheckFailAfterAttempt()
        {
            var antispam = new AntiSpam(TimeSpan.FromSeconds(10), 3);

            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(false, antispam.Check());
            Assert.AreEqual(false, antispam.Check());
        }

        [Test]
        public void T1_CheckPassWhenWaitingEnough()
        {
            var fakeTimeProvider = new FakeTimeProvider();

            var antispam = new AntiSpam(TimeSpan.FromSeconds(10), 3, fakeTimeProvider);
            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(true, antispam.Check());

            fakeTimeProvider.date = fakeTimeProvider.date.AddSeconds(10);

            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(true, antispam.Check());
            Assert.AreEqual(false, antispam.Check());
            Assert.AreEqual(false, antispam.Check());
        }

        [Test]
        public void T1_CheckFailWhenZeroMessages()
        {
            var antispam = new AntiSpam(TimeSpan.FromSeconds(10), 0);
            Assert.AreEqual(false, antispam.Check());
        }
    }
}