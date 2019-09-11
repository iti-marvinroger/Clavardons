using System;
namespace ITI.Clavardons.Providers
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
