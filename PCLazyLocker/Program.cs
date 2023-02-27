using System.Collections.ObjectModel;

namespace PCLazyLocker;

internal class Program
{
    static async Task Main(string[] args)
    {
        var keysAliases = GetKeysAliases();
        var keysCombination = GetKeysCombination();

        var pcLocker = new PCLocker(keysCombination, keysAliases);
        await pcLocker.StartPollingAsync();

        Wait();
    }

    private static ReadOnlyCollection<Keys> GetKeysCombination()
    {
        return new Keys[] { Keys.ControlKey, Keys.Menu, Keys.Escape }
                .AsReadOnly();
    }

    private static ReadOnlyDictionary<Keys, Keys> GetKeysAliases()
    {
        return new Dictionary<Keys, Keys>
        {
            { Keys.LMenu, Keys.Menu },
            { Keys.RMenu, Keys.Menu },
            { Keys.LControlKey, Keys.ControlKey},
            { Keys.RControlKey, Keys.ControlKey },
            { Keys.LShiftKey, Keys.ShiftKey },
            { Keys.RShiftKey, Keys.ShiftKey },
        }.AsReadOnly();
    }

    private static void Wait()
    {
        while (true)
            Thread.Sleep((int)10e6);
    }
}
