using System.Management;

namespace PCLazyLocker;

public static class UsbDeviceMonitor
{
    private static int _devicesCount;

    public delegate void DevicesCountChangedHandle();
    public static event DevicesCountChangedHandle? DevicesCountChanged;

    private static CancellationTokenSource? _connectedDevicesController;

    static UsbDeviceMonitor()
    {
        _devicesCount = GetConnectedDevicesCount();
    }

#pragma warning disable CS1998
    public static async Task DevicesCountStartMonitoringAsync()
#pragma warning restore CS1998
    {
        _connectedDevicesController = new();

#pragma warning disable CS4014
        Task.Run(() =>
        {
            while (true)
            {
                if (CountMonitoringStopRequested())
                    return;

                var devicesCount = GetConnectedDevicesCount();

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

    public static void DevicesCountStopMonitoring()
    {
        _connectedDevicesController?.Cancel();
    }

    private static bool CountMonitoringStopRequested()
    {
        return _connectedDevicesController is not null
            && _connectedDevicesController.IsCancellationRequested;
    }

    private static int GetConnectedDevicesCount()
    {
        using var searcher = new ManagementObjectSearcher(@"select * from Win32_USBHub");

        return searcher.Get().Count;
    }
}
