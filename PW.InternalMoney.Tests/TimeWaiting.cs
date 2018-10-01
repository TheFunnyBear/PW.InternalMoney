using System;
using System.Threading;

namespace PW.InternalMoney.Tests
{
    public sealed class TimeWaiting
    {
        internal static void WaitSeconds(int seconds)
        {
            Thread.Sleep((int)TimeSpan.FromSeconds(seconds).TotalMilliseconds);
        }
    }
}
