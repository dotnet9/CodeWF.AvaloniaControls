using Avalonia.Platform;
using System;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;

public interface INativeWindow
{
    IPlatformHandle CreateWindow(string ProcessPath, IPlatformHandle parent, Func<IPlatformHandle> createDefault);
}