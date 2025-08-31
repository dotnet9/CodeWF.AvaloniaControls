using Avalonia.Controls;
using Avalonia.Platform;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;

public class EmbedWindow(string processPath) : NativeControlHost
{
    public static INativeWindow? Implementation { get; set; }
    private bool _isCreated = false;
    private IPlatformHandle _handle;

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (_isCreated)
        {
            return _handle;
        }

        _isCreated = true;
        _handle = Implementation?.CreateWindow(processPath, parent, () => base.CreateNativeControlCore(parent))
               ?? base.CreateNativeControlCore(parent);
        return _handle;
    }

    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        base.DestroyNativeControlCore(control);
    }
}