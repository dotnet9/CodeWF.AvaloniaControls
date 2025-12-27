namespace CodeWF.AvaloniaControls.Extensions;
public static class WindowExtension
{
    public static void AddEscClose(this Avalonia.Controls.Window window)
    {
        window.KeyDown += (s, e) =>
        {
            if (e.Key == Avalonia.Input.Key.Escape)
            {
                window.Close();
            }
        };
    }
}