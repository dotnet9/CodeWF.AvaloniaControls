# Changelog

## 12.0.2 - 2026-05-02

- Switched the repository to central NuGet package management with `Directory.Packages.props`.
- Migrated the solution to `.slnx` and pinned the .NET 11 preview SDK with `global.json`.
- Upgraded the repository baseline to .NET 11 and the Avalonia 12 package ecosystem.
- Reorganized the repository into an open source friendly layout: all projects now live physically under `src/`, while sample applications are grouped logically in the `.slnx` solution view.
- Renamed sample applications to clearer open source facing names such as `CodeWF.AvaloniaControls.Showcase`, `CodeWF.AvaloniaControls.ProDataGridShowcase`, `CodeWF.AvaloniaControls.FluentStarterDemo`, and `CodeWF.Themes.StatusBadgeDemo`.
- Added the dedicated `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` sample for Avalonia 12 large-table tab and document switching verification.
- Added a root `pack.bat` script for one-click restore, build, and NuGet packing.

## 11.3.14.1 - 2026-04-29

- Fixed DataGrid tooltip column index handling to avoid invalid column/index access.
- Fixed the internal `owner` property handling in `Transfer`'s `SearchListBox`.
- Fixed duplicate reset notifications from `RangeObservableCollection.Clear`.
- Fixed the top placement path rendering issue in the trapezoid tab item border.
- Improved cleanup and timeout handling in `DockReactiveUIDemo` process embedding flows.

## 11.2.1.9 - 2025-07-15

- Added `CodeWF.AvaloniaControls.DataGrid`.

## 11.3.0.12 - 2025-07-14

- Added `CodeWF.AvaloniaControls.Dock`.

## 0.1.0.0 - 2025-06-28

- Added the initial `CodeWF.AvaloniaControls` TabControl extension package.
