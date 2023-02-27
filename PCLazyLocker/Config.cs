using System.Collections.ObjectModel;

namespace PCLazyLocker;

public class Config
{
    public static ReadOnlyDictionary<Keys, Keys> GetKeysAliases()
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

    public static ReadOnlyCollection<Keys> GetKeysCombination()
    {
        return new Keys[] { Keys.ControlKey, Keys.Menu, Keys.Escape }
                .AsReadOnly();
    }
}
