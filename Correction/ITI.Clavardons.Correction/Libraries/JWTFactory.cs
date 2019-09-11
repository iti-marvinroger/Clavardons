using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ITI.Clavardons.Libraries
{
    public struct JWTPayload
    {
        /// <summary>
        /// Unique ID of the user
        /// </summary>
        [JsonProperty(PropertyName = "sub")]
        public string Subject;

        /// <summary>
        /// Name of the user
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name;

        /// <summary>
        /// Unique ID of the JWT
        /// </summary>
        [JsonProperty(PropertyName = "jti")]
        public string JwtID;

        /// <summary>
        /// Serializes the JWT to JSON.
        /// </summary>
        /// <returns>The JSON serialized JWT</returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// Parses a JWT.
        /// </summary>
        /// <param name="payload">The JWT string.</param>
        /// <returns></returns>
        public static JWTPayload Parse(string payload)
        {
            return JsonConvert.DeserializeObject<JWTPayload>(payload);
        }
    }

    /// <summary>
    /// The JWTFactory class allows generating and verifying JWTs.
    /// </summary>
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

        /// <summary>
        /// Generates the JWT corresponding to the given payload.
        /// </summary>
        /// <param name="payload">The payload to generate the JWT from</param>
        /// <returns>The JWT string</returns>
        public string Generate(JWTPayload payload)
        {
            string serializedPayload = Base64UrlEncoder.Encode(payload.Serialize());
            string headerAndPayload = $"{_jwtHeader}.{serializedPayload}";

            string signature = sign(headerAndPayload);

            return $"{headerAndPayload}.{signature}";
        }

        /// <summary>
        /// Verifies whether or not the given JWT is valid.
        /// </summary>
        /// <param name="jwt">JWT string</param>
        /// <returns>Whether or not the JWT is valid</returns>
        public bool Verify(string jwt)
        {
            var splitted = jwt.Split('.');

            var headerAndPayload = $"{splitted[0]}.{splitted[1]}";

            var signature = sign(headerAndPayload);

            return signature == splitted[2];
        }

        /// <summary>
        /// Parses a JWT.
        /// </summary>
        /// <param name="jwt">JWT string</param>
        /// <returns>The parsed JWTPayload</returns>
        public static JWTPayload Parse(string jwt)
        {
            var splitted = jwt.Split('.');
            var payload = splitted[1];

            var plainPayload = Base64UrlEncoder.Decode(payload);

            return JWTPayload.Parse(plainPayload);
        }
    }
}
