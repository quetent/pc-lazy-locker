namespace PCLazyLocker;

public static class MouseManager
{
    public delegate void MouseEventHandler(Keys key);

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

                //Thread.Sleep(250);
                Console.WriteLine(distance);

                (previousX, previousY) = (x, y);

                if (distance > 0)
                    CursorPositionChanged?.Invoke(distance);
            }
        });
#pragma warning restore CS4014 // call is not awaited
    }
}
