namespace PCLazyLocker;

internal class Program
{
    static async Task Main(string[] args)
    {
        var keysAliases = GetKeysAliases();
        var cts = new CancellationTokenSource();

        await KeyboardManager.SetKeysCombination(
                new Keys[] { Keys.ControlKey, Keys.Menu, Keys.Escape },
                () => Console.WriteLine("pressed"), 
                cts.Token,
                keysAliases);

        Wait();
    }

    private static IReadOnlyDictionary<Keys, Keys> GetKeysAliases()
    {
        return new Dictionary<Keys, Keys>
        {
            { Keys.LMenu, Keys.Menu },
            { Keys.RMenu, Keys.Menu },
            { Keys.LControlKey, Keys.ControlKey},
            { Keys.RControlKey, Keys.ControlKey },
            { Keys.LShiftKey, Keys.ShiftKey },
            { Keys.RShiftKey, Keys.ShiftKey },
        }
        .AsReadOnly();
    }

    private static void Wait()
    {
        while (true)
            Thread.Sleep((int)10e6);
    }
}
