@echo off
setlocal enabledelayedexpansion

set "project_paths=src\CodeWF.AvaloniaControls.DataGridLegacyDemo src\CodeWF.AvaloniaControls.ProDataGridPerformanceDemo src\CodeWF.AvaloniaControls.ProDataGridShowcase src\CodeWF.AvaloniaControls.TreeDataGridLegacyDemo src\CodeWF.AvaloniaControls.DockDemo src\CodeWF.AvaloniaControls.DockPrismDemo src\CodeWF.AvaloniaControls.DockReactiveUIDemo src\CodeWF.AvaloniaControls.FluentStarterDemo src\CodeWF.AvaloniaControls.Showcase src\CodeWF.Themes.StatusBadgeDemo"
set "platforms=win-x64 linux-x64"

call "%~dp0publishbase.bat" "%project_paths%" "%platforms%"
