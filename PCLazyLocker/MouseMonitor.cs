namespace PCLazyLocker;

public static class MouseMonitor
{
    public delegate void MousePressedHandler();
    public static event MousePressedHandler? MousePressed;

    public delegate void CursorPositionChangedHandle(double delta);
    public static event CursorPositionChangedHandle? CursorPositionChanged;

    private static CancellationTokenSource? _cursorMonitoringController;

#pragma warning disable CS1998
    public static async Task CursorStartMonitoringAsync()
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        _cursorMonitoringController = new();

        Task.Run(() =>
        {
            var previousX = Cursor.Position.X;
            var previousY = Cursor.Position.Y;

            while (true)
            {
                if (CursorMonitoringStopRequested())
                    return;

                var (x, y) = (Cursor.Position.X, Cursor.Position.Y);
                var (dx, dy) = (x - previousX, y - previousY);
                var delta = Math.Sqrt(dx * dx + dy * dy);

                (previousX, previousY) = (x, y);

                if (delta > 0)
                    CursorPositionChanged?.Invoke(delta);

                Thread.Sleep(Config.POLLING_DELAY_MS);
            }
        });
#pragma warning restore CS4014
    }

    public static void CursorMonitoringStop()
    {
        _cursorMonitoringController?.Cancel();
    }

    private static bool CursorMonitoringStopRequested()
    {
        return _cursorMonitoringController is not null
            && _cursorMonitoringController.IsCancellationRequested;
    }

    public static void ButtonsPressedStartMonitoringAsync()
    {
        InputKeysMonitor.KeyPressed += ButtonPressedHandle;
    }

    public static void ButtonsPressedStopMonitoring()
    {
        InputKeysMonitor.KeyPressed -= ButtonPressedHandle;
    }

    private static void ButtonPressedHandle(Keys pressedKey)
    {
        if (InputKeysMonitor.IsMouseKey(pressedKey))
            MousePressed?.Invoke();
    }
}
