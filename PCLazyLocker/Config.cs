using System.Collections.ObjectModel;

namespace PCLazyLocker;

public static class Config
{
    public const int POLLING_FREQUENCY_MS = 450;

    public static ReadOnlyDictionary<Keys, Keys> GetKeysAliases()
    {
        return new Dictionary<Keys, Keys>
        {
            { Keys.MButton, Keys.LButton },
            { Keys.RButton, Keys.LButton },
            { Keys.XButton1, Keys.LButton },
            { Keys.XButton2, Keys.LButton },
            { Keys.LMenu, Keys.Menu },
            { Keys.RMenu, Keys.Menu },
            { Keys.LControlKey, Keys.ControlKey},
            { Keys.RControlKey, Keys.ControlKey },
            { Keys.LShiftKey, Keys.ShiftKey },
            { Keys.RShiftKey, Keys.ShiftKey },
        }.AsReadOnly();
    }

    public static ReadOnlyCollection<Keys> GetKeysCombination()
    {
        return new Keys[] { Keys.ControlKey, Keys.Menu, Keys.Escape }
                .AsReadOnly();
    }
}
