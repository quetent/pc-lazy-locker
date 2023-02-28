using System.Collections.ObjectModel;

namespace PCLazyLocker;

public static class InputKeysMonitor
{
    public delegate void InputKeyHandler(Keys key);
    public static event InputKeyHandler? KeyPressed;

#pragma warning disable CS1998
    public static async Task StartMonitoringAsync(ReadOnlyDictionary<Keys, Keys>? keysAliases = null)
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        Task.Run(() =>
        {
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

                        KeyPressed?.Invoke(key);
                    }
                }

                Thread.Sleep(Config.POLLING_DELAY_MS);
            }
        });
#pragma warning restore CS4014
    }

    public static bool IsKeyboardKey(Keys key)
    {
        return !IsMouseKey(key);
    }

    public static bool IsMouseKey(Keys key)
    {
        return key is Keys.LButton
            || key is Keys.RButton
            || key is Keys.MButton
            || key is Keys.XButton1
            || key is Keys.XButton2;
    }

    private static bool IsUserInput(int keyState)
    {
        return keyState == 1 || keyState == 32769;
    }

    private static bool IsAlias(ReadOnlyDictionary<Keys, Keys>? keysAliases, Keys key, out Keys replaceKey)
    {
        if (keysAliases is not null && keysAliases.TryGetValue(key, out replaceKey))
            return true;

        replaceKey = Keys.None;
        return false;
    }
}