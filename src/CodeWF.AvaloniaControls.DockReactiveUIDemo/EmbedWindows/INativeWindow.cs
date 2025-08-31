using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;

public interface INativeWindow
{
    IPlatformHandle CreateWindow(IPlatformHandle parent, Func<IPlatformHandle> createDefault);

    void CloseWindow();
}