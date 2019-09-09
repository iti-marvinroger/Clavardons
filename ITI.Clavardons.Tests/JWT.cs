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
        public void T1GenerateSignature()
        {
            byte[] key1 = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload1 = new JWTPayload{ Subject = "12" };

            var jwtFactory = new JWTFactory(key1);
            var jwt = jwtFactory.Generate(payload1);

            Assert.AreEqual(jwt, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMiJ9.YyJFI-9mZc1w-YX3bjPSRr-kJ7nlzPlMNI4cgwm735A");
        }
    }
}