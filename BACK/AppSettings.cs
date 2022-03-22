using System;

namespace BACK
{
    public class AppSettings
    {
        public AppSettings ()
        {
            Secret = Environment.GetEnvironmentVariable("JWT_SECRET");
        }

        public string Secret { get; set; }
        public int Expires { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}