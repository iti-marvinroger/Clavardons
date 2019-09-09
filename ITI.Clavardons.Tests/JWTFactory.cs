using ITI.Clavardons.Libraries;
using NUnit.Framework;

namespace Tests
{
    public class TestJWTFactory
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void T1_GenerateJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload{ Subject = "12" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            Assert.AreEqual("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMiJ9.YyJFI-9mZc1w-YX3bjPSRr-kJ7nlzPlMNI4cgwm735A", jwt);
        }

        [Test]
        public void T2_VerifyGoodJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload { Subject = "24" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            Assert.AreEqual(true, jwtFactory.Verify(jwt));
        }

        [Test]
        public void T3_VerifyBadJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload { Subject = "24" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            Assert.AreEqual(false, jwtFactory.Verify(jwt + "deadbeef"));
        }

        [Test]
        public void T4_ParseJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload { Subject = "42" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            var parsed = JWTFactory.Parse(jwt);

            Assert.AreEqual("42", parsed.Subject);
            Assert.AreEqual(null, parsed.ExpirationTime);
            Assert.AreEqual(null, parsed.JwtID);
        }
    }
}