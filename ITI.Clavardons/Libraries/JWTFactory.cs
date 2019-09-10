using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ITI.Clavardons.Libraries
{
    public struct JWTPayload
    {
        [JsonProperty(PropertyName = "sub")]
        public string Subject;
        [JsonProperty(PropertyName = "name")]
        public string Name;
        [JsonProperty(PropertyName = "jti")]
        public string JwtID;

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static JWTPayload Parse(string payload)
        {
            return JsonConvert.DeserializeObject<JWTPayload>(payload);
        }
    }

    public class JWTFactory
    {
        private readonly string _jwtHeader = Base64UrlEncoder.Encode(@"{""alg"":""HS256"",""typ"":""JWT""}");
        private byte[] _secretkey;

        public JWTFactory(byte[] secretKey)
        {
            _secretkey = secretKey;
        }

        private string sign(string payload)
        {
            using (HMACSHA256 hmac = new HMACSHA256(_secretkey))
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(payload);
                var signatureBytes = hmac.ComputeHash(plainTextBytes);

                return Base64UrlEncoder.Encode(signatureBytes);
            }
        }

        public string Generate(JWTPayload payload)
        {
            string serializedPayload = Base64UrlEncoder.Encode(payload.Serialize());
            string headerAndPayload = $"{_jwtHeader}.{serializedPayload}";

            string signature = sign(headerAndPayload);

            return $"{headerAndPayload}.{signature}";
        }

        public bool Verify(string jwt)
        {
            var splitted = jwt.Split('.');

            var headerAndPayload = $"{splitted[0]}.{splitted[1]}";

            var signature = sign(headerAndPayload);

            return signature == splitted[2];
        }

        public static JWTPayload Parse(string jwt)
        {
            var splitted = jwt.Split('.');
            var payload = splitted[1];

            var plainPayload = Base64UrlEncoder.Decode(payload);

            return JWTPayload.Parse(plainPayload);
        }
    }
}
