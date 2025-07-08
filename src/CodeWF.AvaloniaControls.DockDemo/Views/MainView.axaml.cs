using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using CodeWF.AvaloniaControls.DockDemo.ViewModels;
using Dock.Model;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Serializer;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeWF.AvaloniaControls.DockDemo.Views;

public partial class MainView : UserControl
{
    private IDockSerializer? _serializer;
    private IDockState? _dockState;
    public MainView()
    {
        InitializeComponent();
        InitializeDockState();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InitializeDockState()
    {
        _serializer = new DockSerializer(typeof(ObservableCollection<>));
        _dockState = new DockState();

        if (DataContext is MainViewModel mainViewModel)
        {
            var layout = mainViewModel.Layout;
            if (layout != null)
            {
                _dockState.Save(layout);
            }
        }
    }

    private async Task OpenLayout()
    {
        if (_serializer is null || _dockState is null)
        {
            return;
        }

        var storageProvider = (this.GetVisualRoot() as TopLevel)?.StorageProvider;
        if (storageProvider is null)
        {
            return;
        }

        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open layout",
            FileTypeFilter = [new FilePickerFileType("Json") { Patterns = ["*.json"] }, new FilePickerFileType("All") { Patterns = ["*.*"] }],
            AllowMultiple = false
        });

        var file = result.FirstOrDefault();

        if (file is not null)
        {
            try
            {
                await using var stream = await file.OpenReadAsync();
                using var reader = new StreamReader(stream);
                var layout = _serializer.Load<IRootDock?>(stream);
                if (layout is not null)
                {
                    _dockState.Restore(layout);

                    if (DataContext is MainViewModel mainViewModel)
                    {
                        mainViewModel.Layout = layout;
                        mainViewModel.InitLayout();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private async Task SaveLayout()
    {
        if (_serializer is null || _dockState is null)
        {
            return;
        }

        var storageProvider = (this.GetVisualRoot() as TopLevel)?.StorageProvider;
        if (storageProvider is null)
        {
            return;
        }

        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save layout",
            FileTypeChoices = [new FilePickerFileType("Json") { Patterns = ["*.json"] }, new FilePickerFileType("All") { Patterns = ["*.*"] }],
            SuggestedFileName = "layout",
            DefaultExtension = "json",
            ShowOverwritePrompt = true
        });

        if (file is not null)
        {
            try
            {
                await using var stream = await file.OpenWriteAsync();

                if (DataContext is MainViewModel mainViewModel)
                {
                    _serializer.Save(stream, mainViewModel.Layout);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private void CloseLayout()
    {
        if (DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.CloseLayout();
            mainViewModel.Layout = null;
        }
    }

    private async void FileOpenLayout_OnClick(object? sender, RoutedEventArgs e)
    {
        await OpenLayout();
    }

    private async void FileSaveLayout_OnClick(object? sender, RoutedEventArgs e)
    {
        await SaveLayout();
    }

    private void FileCloseLayout_OnClick(object? sender, RoutedEventArgs e)
    {
        CloseLayout();
    }
}