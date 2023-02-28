using System.Management;

namespace PCLazyLocker;

public static class UsbDevicesManager
{
    private static int _devicesCount;

    public delegate void DevicesCountChangedHandle();
    public static event DevicesCountChangedHandle? DevicesCountChanged;

    private static CancellationTokenSource? _connectedDevicesController;

    static UsbDevicesManager()
    {
        _devicesCount = GetConnectedDevicedCount();
    }

#pragma warning disable CS1998
    public static async Task DevicesCountChangingStartMonitoringAsync()
#pragma warning restore CS1998
    {
        _connectedDevicesController = new();

#pragma warning disable CS4014
        Task.Run(() =>
        {
            while (true)
            {
                if (DeviceCountChangingStopRequested())
                    return;

                var devicesCount = GetConnectedDevicedCount();

                if (_devicesCount != devicesCount)
                {
                    DevicesCountChanged?.Invoke();
                    _devicesCount = devicesCount;
                }

                Thread.Sleep(Config.POLLING_DELAY_MS);
            };
        });
#pragma warning restore CS4014
    }

    public static void DevicesCountChangingStopMonitoring()
    {
        _connectedDevicesController?.Cancel();
    }

    private static bool DeviceCountChangingStopRequested()
    {
        return _connectedDevicesController is not null
            && _connectedDevicesController.IsCancellationRequested;
    }

    private static int GetConnectedDevicedCount()
    {
        using var searcher = new ManagementObjectSearcher(@"select * from Win32_USBHub");
        using var devices = searcher.Get();

        return devices.Count;
    }
}
