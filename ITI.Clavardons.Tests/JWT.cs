using ITI.Clavardons.Libraries;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();

            byte[] key1 = { 0, 1, 2 };
            var payload1 = new JWTPayload{ Subject = "12" };

            var jwtFactory = new JWTFactory(key1);
            var jwt = jwtFactory.Generate(payload1);

            Assert.Equals(jwt, "abc.lol");
        }
    }
}