namespace JustAnEmailClient.Helpers;

public class TimeoutHelper
{
    public static void SetTimeout(Action action, int milliseconds)
    {
        Task.Delay(milliseconds).ContinueWith((task) =>
        {
            action();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
