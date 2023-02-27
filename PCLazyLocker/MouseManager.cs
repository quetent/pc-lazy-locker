using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCLazyLocker;

public class MouseManager
{
    public static async Task SetMouseHandler(
            ReadOnlyCollection<Keys> keys,
            Action handler,
            CancellationToken cancellationToken,
            ReadOnlyDictionary<Keys, Keys>? keysAliases = default)
    {

    }
}
