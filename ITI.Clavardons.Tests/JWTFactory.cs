using FluentAssertions;
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

            jwt.Should().Be("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMiJ9.YyJFI-9mZc1w-YX3bjPSRr-kJ7nlzPlMNI4cgwm735A");
        }

        [Test]
        public void T2_VerifyGoodJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload { Subject = "24" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            jwtFactory.Verify(jwt).Should().BeTrue();
        }

        [Test]
        public void T3_VerifyBadJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload { Subject = "24" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            jwtFactory.Verify(jwt + "deadbeef").Should().BeFalse();
        }

        [Test]
        public void T4_ParseJWT()
        {
            byte[] key = { 0xDE, 0xAD, 0xBE, 0xEF };
            var payload = new JWTPayload { Subject = "42" };

            var jwtFactory = new JWTFactory(key);
            var jwt = jwtFactory.Generate(payload);

            var parsed = JWTFactory.Parse(jwt);

            parsed.Subject.Should().Be("42");
            parsed.JwtID.Should().BeNull();
        }
    }
}