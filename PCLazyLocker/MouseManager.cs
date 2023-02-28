namespace PCLazyLocker;

public static class MouseManager
{
    public delegate void MousePressedHandler();
    public static event MousePressedHandler? MousePressed;

    public delegate void CursorPositionChangedHandle(double distance);
    public static event CursorPositionChangedHandle? CursorPositionChanged;

    private static CancellationTokenSource? _cursorMovingController;

#pragma warning disable CS1998
    public static async Task CursorMovingStartMonitoringAsync()
#pragma warning restore CS1998
    {
#pragma warning disable CS4014
        _cursorMovingController = new();

        Task.Run(() =>
        {
            var previousX = Cursor.Position.X;
            var previousY = Cursor.Position.Y;

            while (true)
            {
                if (CursorMovingStopRequested())
                    return;

                var (x, y) = (Cursor.Position.X, Cursor.Position.Y);
                var (dx, dy) = (x - previousX, y - previousY);
                var distance = Math.Sqrt(dx * dx + dy * dy);

                (previousX, previousY) = (x, y);

                if (distance > 0)
                    CursorPositionChanged?.Invoke(distance);

                Thread.Sleep(Config.POLLING_DELAY_MS);
            }
        });
#pragma warning restore CS4014
    }

    public static void CursorMovingStopMonitoring()
    {
        _cursorMovingController?.Cancel();
    }

    private static bool CursorMovingStopRequested()
    {
        return _cursorMovingController is not null
            && _cursorMovingController.IsCancellationRequested;
    }

    public static void MouseButtonsPressedStartMonitoringAsync()
    {
        InputKeysMonitor.KeyPressed += MouseButtonPressedHandler;
    }

    public static void MouseButtonsPressedStopMonitoring()
    {
        InputKeysMonitor.KeyPressed -= MouseButtonPressedHandler;
    }

    private static void MouseButtonPressedHandler(Keys pressedKey)
    {
        if (InputKeysMonitor.IsMouseKey(pressedKey))
            MousePressed?.Invoke();
    }
}
