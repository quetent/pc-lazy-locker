namespace PCLazyLocker;

public static class BooleanExtensions
{
    public static void Switch(ref this bool value)
    {
        value = !value;
    }
}
