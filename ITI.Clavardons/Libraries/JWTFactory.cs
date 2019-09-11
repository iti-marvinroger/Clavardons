using System;
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

        /// <summary>
        /// Generates the JWT corresponding to the given payload.
        /// </summary>
        /// <param name="payload">The payload to generate the JWT from</param>
        /// <returns>The JWT string</returns>
        public string Generate(JWTPayload payload)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies whether or not the given JWT is valid.
        /// </summary>
        /// <param name="jwt">JWT string</param>
        /// <returns>Whether or not the JWT is valid</returns>
        public bool Verify(string jwt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a JWT.
        /// </summary>
        /// <param name="jwt">JWT string</param>
        /// <returns>The parsed JWTPayload</returns>
        public static JWTPayload Parse(string jwt)
        {
            throw new NotImplementedException();
        }
    }
}
