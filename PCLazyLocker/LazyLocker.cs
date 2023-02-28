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

        _keysCombination = new KeysCombination(keysCombination, KeyCombinationPressedHandler, MissedKeyPressedHandler);
        _keysAliases = keysAliases;
    }

#pragma warning disable CS1998
    public async Task WaitLockAsync()
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        MouseManager.MousePressed += MouseButtonPressedHandler;
        MouseManager.CursorPositionChanged += CursorPositionChangedHandle;
        UsbDevicesManager.DevicesCountChanged += UsbDevicesCountChanged;

        InputKeysMonitor.StartMonitoringAsync(_keysAliases);
        _keysCombination.StartMonitoringAsync();
#pragma warning restore CS4014
    }

#pragma warning disable CS1998
    private static async Task StartMonitoringDevices()
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        MouseManager.MouseButtonsPressedStartMonitoringAsync();
        MouseManager.CursorMovingStartMonitoringAsync();
        UsbDevicesManager.DevicesCountChangingStartMonitoringAsync();
#pragma warning restore CS4014
    }

    private static void StopMonitoringDevices()
    {
        MouseManager.MouseButtonsPressedStopMonitoring();
        MouseManager.CursorMovingStopMonitoring();
        UsbDevicesManager.DevicesCountChangingStopMonitoring();
    }

    private async void KeyCombinationPressedHandler()
    {
        _isLocked.Switch();

        if (!_isLocked)
        {
            NotificationManager.SendNotification("Lazy lock disable", string.Empty);
            StopMonitoringDevices();
        }
        else
        {
            NotificationManager.SendNotification("Lazy lock enable", string.Empty);
            await StartMonitoringDevices();
        }
    }

    private void MissedKeyPressedHandler()
    {
        if (_isLocked)
            LockPC(LockReason.KeyboardPress);
    }

    private void MouseButtonPressedHandler()
    {
        if (_isLocked)
            LockPC(LockReason.MouseButtonPress);
    }

    private void CursorPositionChangedHandle(double distance)
    {
        if (_isLocked && distance > 2)
            LockPC(LockReason.CursorMoving);
    }

    private void UsbDevicesCountChanged()
    {
        if (_isLocked)
            LockPC(LockReason.UsbDeviceCountChanged);
    }

    private void LockPC(LockReason reason)
    {
        lock (this)
        {
            if (_isLocked)
            {
                //NativeMethods.LockPC();
                NotificationManager.SendNotification("PC was locked", $"Reason: {reason}");

                _isLocked.Switch();
                StopMonitoringDevices();
            }
        }
    }
}
