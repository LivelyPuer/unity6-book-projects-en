using System;

// Pop-up messages: game logic posts them, the UI shows them
public static class Toast
{
    public static event Action<string> Shown;
    public static void Show(string message) => Shown?.Invoke(message);
}
