using System.Collections.ObjectModel;

namespace PCLazyLocker;

public class LazyLocker
{
    private bool _isLocked;

    private readonly ReadOnlyCollection<Keys> _keysCombination;
    private readonly ReadOnlyDictionary<Keys, Keys>? _keysAliases;

    public LazyLocker(
        ReadOnlyCollection<Keys> keysCombination,
        ReadOnlyDictionary<Keys, Keys>? keysAliases = null)
    {
        _isLocked = false;

        _keysCombination = keysCombination;
        _keysAliases = keysAliases;
    }

#pragma warning disable CS1998 // async method lacks await
    public async Task Start()
#pragma warning restore CS1998 // async method lacks await
    {
#pragma warning disable CS4014 // call is not awaited
        InputKeysMonitor.StartMonitoringAsync(_keysAliases);

        MouseManager.CursorPositionChanged += CursorChangedPositionHandle;
        MouseManager.CursorMovingStartMonitoringAsync();

        var keysCombination = new KeysCombination(_keysCombination, () => _isLocked.Switch());
        keysCombination.StartMonitoringAsync();


#pragma warning restore CS4014 // call is not awaited
    }

    private void A()
    {

    }

    private void CursorChangedPositionHandle(double distance)
    {
        if (distance > 2)
            LockPC();
    }

    private void LockPC()
    {
        //NativeMethods.LockPC();
        if (_isLocked)
            Console.WriteLine("pc locked");
    }
}
