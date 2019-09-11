using System;
using FluentAssertions;
using ITI.Clavardons.Libraries;
using ITI.Clavardons.Providers;
using NUnit.Framework;

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

            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeFalse();
            antispam.Check().Should().BeFalse();
        }

        [Test]
        public void T2_CheckPassWhenWaitingEnough()
        {
            var fakeTimeProvider = new FakeTimeProvider();

            var antispam = new AntiSpam(TimeSpan.FromSeconds(10), 3, fakeTimeProvider);
            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeTrue();

            fakeTimeProvider.date = fakeTimeProvider.date.AddSeconds(10);

            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeTrue();
            antispam.Check().Should().BeFalse();
            antispam.Check().Should().BeFalse();
        }

        [Test]
        public void T3_CheckFailWhenZeroMessages()
        {
            var antispam = new AntiSpam(TimeSpan.FromSeconds(10), 0);
            antispam.Check().Should().BeFalse();
        }
    }
}