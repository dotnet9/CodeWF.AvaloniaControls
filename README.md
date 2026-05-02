# CodeWF.AvaloniaControls

| Name | NuGet | Download |
|------|-------|----------|
| CodeWF.AvaloniaControls | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls)](https://www.nuget.org/packages/CodeWF.AvaloniaControls/) |
| CodeWF.AvaloniaControls.DataGrid | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.DataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.DataGrid/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.DataGrid.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.DataGrid/) |
| CodeWF.AvaloniaControls.Dock | [![NuGet](https://img.shields.io/nuget/v/CodeWF.AvaloniaControls.Dock.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Dock/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.AvaloniaControls.Dock.svg)](https://www.nuget.org/packages/CodeWF.AvaloniaControls.Dock/) |
| CodeWF.Core | [![NuGet](https://img.shields.io/nuget/v/CodeWF.Core.svg)](https://www.nuget.org/packages/CodeWF.Core/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.Core.svg)](https://www.nuget.org/packages/CodeWF.Core/) |
| CodeWF.Themes | [![NuGet](https://img.shields.io/nuget/v/CodeWF.Themes.svg)](https://www.nuget.org/packages/CodeWF.Themes/) | [![NuGet](https://img.shields.io/nuget/dt/CodeWF.Themes.svg)](https://www.nuget.org/packages/CodeWF.Themes/) |

A custom Avalonia UI control library and sample repository built on .NET 11 and Avalonia 12.

English | [简体中文](README.zh-CN.md)

## Install

```shell
Install-Package CodeWF.AvaloniaControls
```

## Repository Layout

- `src/`: all project directories, including packable libraries and runnable sample applications
- `docs/`: screenshots, GIFs, and documentation assets
- `artifacts/`: build and package outputs
- The `.slnx` solution groups samples logically into `datagrid`, `docking`, and `showcase`

## Package Strategy

- Central package management is enabled for the main Avalonia 12 repository line through `Directory.Packages.props`
- Projects that must stay on a special package line use explicit package versions in their own `.csproj` files instead of adding conditional branches to the central package manager
- The repository solution uses `.slnx`
- All physical project directories stay under `src/`

## DataGrid Strategy

### Main Avalonia 12 sample line

- All runnable sample applications in the solution stay on the Avalonia 12 main line
- `CodeWF.AvaloniaControls.ProDataGridShowcase` shows functional ProDataGrid scenarios such as tri-state sorting, grouped headers, and dynamic columns
- `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` focuses on large dataset tab switching and document-style switching performance
- The Dock showcase samples no longer depend on `DataGrid` or `TreeDataGrid`, so ordinary Avalonia 12 samples are not blocked by legacy grid packages

### Legacy free DataGrid / TreeDataGrid line

- `CodeWF.AvaloniaControls.DataGrid` is kept as a compatibility package for the last free official `Avalonia.Controls.DataGrid` and `Avalonia.Controls.TreeDataGrid` line
- It no longer relies on central package management and keeps its own explicit Avalonia 11-compatible package references
- `CodeWF.AvaloniaControls.DataGridLegacyDemo` and `CodeWF.AvaloniaControls.TreeDataGridLegacyDemo` are dedicated large-data tab switching samples for the legacy free grid line

### Open-source high-performance DataGrid line

- `CodeWF.AvaloniaControls.ProDataGrid` is a helper package built on top of the MIT-licensed [ProDataGrid](https://www.nuget.org/packages/ProDataGrid/)
- It provides reusable helpers for tri-state sorting, large-dataset presets, and smart tooltips on the Avalonia 12 line

## Open Source Notes

- Commercial package lines are intentionally avoided in this repository
- `Prism.DryIoc.Avalonia` is pinned to `8.1.97.11073` because the `9.x` line is commercial
- `Semi.Avalonia.DataGrid` and `Semi.Avalonia.TreeDataGrid` use their last free open-source theme package versions

## Pack

Run the repository root script to restore, build, and pack all publishable libraries into `artifacts/packages`:

```bat
pack.bat
```

## Samples

- `CodeWF.AvaloniaControls.Showcase`: general control showcase
- `CodeWF.AvaloniaControls.ProDataGridShowcase`: Avalonia 12 ProDataGrid functional sample
- `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo`: Avalonia 12 ProDataGrid large-data switching sample
- `CodeWF.AvaloniaControls.DataGridLegacyDemo`: legacy free DataGrid switching sample
- `CodeWF.AvaloniaControls.TreeDataGridLegacyDemo`: legacy free TreeDataGrid switching sample
- `CodeWF.AvaloniaControls.DockDemo` / `CodeWF.AvaloniaControls.DockPrismDemo` / `CodeWF.AvaloniaControls.DockReactiveUIDemo`: Dock integration samples
- `CodeWF.AvaloniaControls.FluentStarterDemo`: lightweight Fluent-based starter window sample
- `CodeWF.Themes.StatusBadgeDemo`: focused status badge sample built on the themes package

## Demo

### Transfer

![](docs/Images/Transfer.gif)

### ComboBox

![](docs/Images/ComboBox.gif)

### TabControl

![](docs/Images/TabControl.gif)
