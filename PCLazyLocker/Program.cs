namespace PCLazyLocker;

internal class Program
{
#pragma warning disable CS1998 // async method lacks await
    static async Task Main()
#pragma warning restore CS1998 // async method lacks await
    {

        var keysCombination = Config.GetKeysCombination();
        var keysAliases = Config.GetKeysAliases();

        var lazyLocker = new LazyLocker(keysCombination, keysAliases);
#pragma warning disable CS4014 // call is not awaited
        lazyLocker.WaitLockAsync();
#pragma warning restore CS4014 // call is not awaited

        Wait();
    }

    private static void Wait()
    {
        while (true)
            Thread.Sleep((int)10e6);
    }
}
