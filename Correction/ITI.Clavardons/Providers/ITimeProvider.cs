using System;
namespace ITI.Clavardons.Providers
{
    public interface ITimeProvider
    {
        DateTime GetNow();
    }
}
