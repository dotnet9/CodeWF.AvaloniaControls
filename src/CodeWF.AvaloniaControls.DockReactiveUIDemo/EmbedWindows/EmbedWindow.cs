using Avalonia.Controls;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows.Windows;

namespace CodeWF.AvaloniaControls.DockReactiveUIDemo.EmbedWindows;

public class EmbedWindow : NativeControlHost
{
    public INativeWindow? Implementation { get; set; }
    private bool _isCreated = false;
    private IPlatformHandle _handle;

    private static List<EmbedWindow> _allProcesses = new();

    public EmbedWindow(string processPath)
    {
        if (OperatingSystem.IsWindows())
        {
            Implementation = new EmbedWin(processPath);
        }

        _allProcesses.Add(this);
    }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        if (_isCreated)
        {
            return _handle;
        }

        _isCreated = true;
        _handle = Implementation?.CreateWindow(parent, () => base.CreateNativeControlCore(parent))
                  ?? base.CreateNativeControlCore(parent);
        return _handle;
    }

    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        base.DestroyNativeControlCore(control);
    }

    public static void CloseAll()
    {
        if (_allProcesses?.Any() == true)
        {
            foreach (var embedWindow in _allProcesses)
            {
                embedWindow?.Implementation?.CloseWindow();
            }
        }
    }
}