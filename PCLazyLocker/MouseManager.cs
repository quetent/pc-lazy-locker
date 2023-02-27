namespace PCLazyLocker;

public static class MouseManager
{
    public delegate void MousePressedHandler(Keys key);
    public static event MousePressedHandler? MousePressed;

    public delegate void CursorPositionChangedHandle(double distance);
    public static event CursorPositionChangedHandle? CursorPositionChanged;

#pragma warning disable CS1998 // async method lacks await
    public static async Task CursorMovingStartMonitoringAsync()
#pragma warning restore CS1998 // async method lacks await
    {
#pragma warning disable CS4014 // call is not awaited
        Task.Run(() =>
        {
            var previousX = Cursor.Position.X;
            var previousY = Cursor.Position.Y;

            while (true)
            {
                var (x, y) = (Cursor.Position.X, Cursor.Position.Y);
                var (dx, dy) = (x - previousX, y - previousY);
                var distance = Math.Sqrt(dx * dx + dy * dy);

                (previousX, previousY) = (x, y);

                if (distance > 0)
                    CursorPositionChanged?.Invoke(distance);

                Thread.Sleep(Config.POLLING_FREQUENCY_MS);
            }
        });
#pragma warning restore CS4014 // call is not awaited
    }

#pragma warning disable CS1998 // async method lacks await
    public static async Task MousePressedStartMonitoringAsync()
#pragma warning restore CS1998 // async method lacks await
    {
        InputKeysMonitor.KeyPressed += (Keys pressedKey) =>
        {
            if (IsMouseKey(pressedKey))
                MousePressed?.Invoke(pressedKey);
        };
    }

    public static bool IsMouseKey(Keys key)
    {
        return key is Keys.LButton
            || key is Keys.RButton
            || key is Keys.MButton
            || key is Keys.XButton1
            || key is Keys.XButton2;
    }
}
