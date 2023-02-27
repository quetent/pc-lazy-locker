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
    public async Task WaitLockAsync()
#pragma warning restore CS1998 // async method lacks await
    {
#pragma warning disable CS4014 // call is not awaited
        InputKeysMonitor.StartMonitoringAsync(_keysAliases);

        var keysCombination = new KeysCombination(_keysCombination, () => _isLocked.Switch(), MissedKeyPressedHandler);
        keysCombination.StartMonitoringAsync();

        MouseManager.MousePressed += MouseButtonPressedHandler;
        MouseManager.MousePressedStartMonitoringAsync();

        MouseManager.CursorPositionChanged += CursorPositionChangedHandle;
        MouseManager.CursorMovingStartMonitoringAsync();

        UsbDevicesManager.DevicesCountChanged += UsbDevicesCountChanged;
        UsbDevicesManager.DevicesCountChangingStartMonitoringAsync();
#pragma warning restore CS4014 // call is not awaited
    }

    private void MissedKeyPressedHandler()
    {
        if (_isLocked)
            LockPC();
    }

    private void MouseButtonPressedHandler(Keys _)
    {
        if (_isLocked)
            LockPC();
    }

    private void CursorPositionChangedHandle(double distance)
    {
        if (_isLocked && distance > 2)
            LockPC();
    }

    private void UsbDevicesCountChanged()
    {
        if (_isLocked)
            LockPC();
    }

    private void LockPC()
    {
        lock (this)
        {
            if (_isLocked)
            {
                //NativeMethods.LockPC();
                Console.WriteLine("pc locked");
                _isLocked.Switch();
            }
        }
    }
}
