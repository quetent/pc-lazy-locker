using Microsoft.Toolkit.Uwp.Notifications;

namespace PCLazyLocker;

public static class NotificationManager
{
    public static void SendNotification(string title, string text)
    {
        new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(title)
                .AddText(text)
                .Show();
    }
}
