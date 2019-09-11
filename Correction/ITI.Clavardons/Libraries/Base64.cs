using System;
namespace ITI.Clavardons.Libraries
{
    public class Base64
    {
        public static string Encode(byte[] payload)
        {
            return System.Convert.ToBase64String(payload);
        }

        public static string Encode(string payload)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(payload);
            return Encode(plainTextBytes);
        }

        public static string Decode(string base64)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
