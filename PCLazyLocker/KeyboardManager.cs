using System.Collections.ObjectModel;

namespace PCLazyLocker;

public static class KeyboardManager
{
#pragma warning disable CS1998 // async method lacks await 
    public static async Task SetKeysCombination(
#pragma warning restore CS1998
            ReadOnlyCollection<Keys> keys,
            Action handler,
            CancellationToken cancellationToken,
            ReadOnlyDictionary<Keys, Keys>? keysAliases = default)
    {
#pragma warning disable CS4014 // this call is not awaited
        Task.Run(() =>
        {
            while (true)
                if (IsCombinationPressed(keys, keysAliases))
                    handler.Invoke();
        },
        cancellationToken);
#pragma warning restore CS4014
    }

    private static bool IsCombinationPressed(ReadOnlyCollection<Keys> keys, ReadOnlyDictionary<Keys, Keys>? keysAliases)
    {
        var keyAwaitingIndex = 0;
        var previousKey = Keys.None;

        while (true)
        {
            for (var vKey = 0; vKey < 255; vKey++)
            {
                var keyState = NativeMethods.GetKeyState(vKey);

                if (IsUserInput(keyState))
                {
                    var key = (Keys)vKey;

                    if (IsAlias(keysAliases, key, out Keys replaceKey))
                        key = replaceKey;

                    if (key == previousKey)
                        continue;

                    previousKey = key;

                    if (key == keys[keyAwaitingIndex])
                    {
                        keyAwaitingIndex++;

                        if (keyAwaitingIndex == keys.Count)
                            return true;
                    }
                    else
                        keyAwaitingIndex = 0;
                }
            }
        }
    }

    private static bool IsUserInput(int keyState)
    {
        return keyState == 1 || keyState == 32769;
    }

    private static bool IsAlias(ReadOnlyDictionary<Keys, Keys>? keysAliases, Keys key, out Keys replaceKey)
    {
        if (keysAliases is null)
        {
            replaceKey = Keys.None;
            return false;
        }

        if (keysAliases.TryGetValue(key, out replaceKey))
            return true;

        replaceKey = Keys.None;
        return false;
    }
}
