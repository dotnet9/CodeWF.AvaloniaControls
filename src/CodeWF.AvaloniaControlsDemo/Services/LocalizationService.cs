using Lang.Avalonia;

namespace CodeWF.AvaloniaControlsDemo.Services;

internal static class LocalizationService
{
    public static string Get(string key)
    {
        return I18nManager.Instance.GetResource(key) ?? key;
    }
}