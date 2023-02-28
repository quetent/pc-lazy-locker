using System.Collections.ObjectModel;

namespace PCLazyLocker;

public class LazyLocker
{
    private bool _isLocked;

    private readonly KeysCombination _keysCombination;
    private readonly ReadOnlyDictionary<Keys, Keys>? _keysAliases;

    public LazyLocker(
        ReadOnlyCollection<Keys> keysCombination,
        ReadOnlyDictionary<Keys, Keys>? keysAliases = null)
    {
        _isLocked = false;

        _keysCombination = new KeysCombination(keysCombination, KeyCombinationPressedHandle, MissedKeyPressedHandle);
        _keysAliases = keysAliases;
    }

#pragma warning disable CS1998
    public async Task SetLockersAsync()
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        MouseMonitor.MousePressed += MouseButtonPressedHandle;
        MouseMonitor.CursorPositionChanged += CursorPositionChangedHandle;

        InputKeysMonitor.StartMonitoringAsync(_keysAliases);
        _keysCombination.StartMonitoringAsync();
#pragma warning restore CS4014
    }

#pragma warning disable CS1998
    private static async Task StartMonitoringDevices()
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        MouseMonitor.ButtonsPressedStartMonitoringAsync();
        MouseMonitor.CursorStartMonitoringAsync();
#pragma warning restore CS4014
    }

    private static void StopMonitoringDevices()
    {
        MouseMonitor.ButtonsPressedStopMonitoring();
        MouseMonitor.CursorMonitoringStop();
    }

    private async void KeyCombinationPressedHandle()
    {
        _isLocked.Switch();

        if (!_isLocked)
        {
            StopMonitoringDevices();
            NotificationManager.SendNotification("Lazy lock disable", string.Empty);
        }
        else
        {
            await StartMonitoringDevices();
            NotificationManager.SendNotification("Lazy lock enable", string.Empty);
        }
    }

    private void MissedKeyPressedHandle()
    {
        if (_isLocked)
            LockPC(LockReason.KeyboardPress);
    }

    private void MouseButtonPressedHandle()
    {
        if (_isLocked)
            LockPC(LockReason.MouseButtonPress);
    }

    private void CursorPositionChangedHandle(double delta)
    {
        if (_isLocked && delta > 2)
            LockPC(LockReason.CursorMoving);
    }

    private void LockPC(LockReason reason)
    {
        lock (this)
        {
            StopMonitoringDevices();
            NativeMethods.LockPC();

            NotificationManager.SendNotification("PC was locked", $"Reason: {reason}");
            _isLocked.Switch();
        }
    }
}
