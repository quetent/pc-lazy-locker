using System.Management;

namespace PCLazyLocker;

public static class UsbDevicesManager
{
    private static int _devicesCount;

    public delegate void DevicesCountChangedHandle();
    public static event DevicesCountChangedHandle? DevicesCountChanged;

    public static void DevicesCountChangingStartMonitoringAsync()
    {
        Task.Run(() =>
        {
            while (true)
            {
                using var searcher = new ManagementObjectSearcher(@"select * from Win32_USBHub");
                var devicesCount = searcher.Get().Count;

                if (_devicesCount != devicesCount)
                {
                    DevicesCountChanged?.Invoke();
                    _devicesCount = devicesCount;
                }

                Thread.Sleep(Config.POLLING_FREQUENCY_MS);
            };
        });
    }
}
