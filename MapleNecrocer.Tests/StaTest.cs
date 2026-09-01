using System.Runtime.ExceptionServices;
using Xunit;

namespace MapleNecrocer.Tests;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ThemeTestCollection
{
    public const string Name = "Theme tests";
}

internal static class StaTest
{
    internal static void Run(Action action)
    {
        Exception? exception = null;
        var thread = new Thread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
