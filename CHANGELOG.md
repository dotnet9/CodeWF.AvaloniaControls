# Changelog

## 12.0.2.3 - 2026-05-06

- Added `CodeWF.Markdown.Lite`, `CodeWF.Markdown.Lite.Themes`, and `CodeWF.Markdown.Lite.Sample` as lightweight Markdown packages and sample app.
- Removed SVG rendering, multi-language resources, CSharpMath, and TextMate dependencies from the Lite Markdown control line; code blocks and math content now render with lightweight text fallbacks.
- Added Win7 AOT compatibility settings to `CodeWF.Markdown.Lite.Sample` with `YY-Thunks`, `VC-LTL`, Windows 6.1 target metadata, and Win7-compatible application manifest entries.
- Added `CodeWF.Markdown.Lite.Sample` to the Markdown publish script and the solution.

## 12.0.2.1 - 2026-05-05

- Bumped `CodeWF.Markdown` and `CodeWF.Markdown.Themes` to 12.0.2.1.
- Removed the `MarkdownViewer.Value` compatibility API so `MarkdownViewer` only accepts Markdown text through `Markdown`.
- Added Markdown SVG image rendering and local missing-file fallback text.
- Expanded README documentation for Markdown package installation, `MarkdownViewer` XAML usage, typography theme registration, runtime theme switching, and image path handling.
- Added XML documentation for the Markdown viewer, code-block tool render event arguments, theme entry point, and typography theme keys.
- Added a dedicated `publish_Markdown.bat` script and included it in the solution file.
- Removed the unused `GroupItems` compatibility collection from the ProDataGrid cross rows/columns showcase view model.

## 12.0.2 - 2026-05-05

- Split package versioning into five independently maintained version groups: controls/themes, DataGrid, Dock, ProDataGrid, and Markdown/themes.
- Added Markdown packages to the root packing script so `CodeWF.Markdown` and `CodeWF.Markdown.Themes` are emitted with the other library packages.
- Added a `GridSplitter` between the Markdown sample editor and preview panes for live layout testing.
- Improved Markdown rendering for bottom padding, long link wrapping, Markdig task/reference nodes, incremental stress testing, and code-block selection readability.
- Removed the `MarkdownViewer.BasePath` file-path API so the viewer surface stays Markdown-text driven.
- Enhanced the Markdown sample with Chinese incremental replacement, insertion, and append stress scenarios.
- Added the Markdown sample to the one-click publish script with Windows/Linux publish profiles and a `net11.0-windows` target.
- Split `CodeWF.AvaloniaControls` into a core control package and the new `CodeWF.AvaloniaControls.Themes` template/resource package.
- Moved `StatusBadge` from the obsolete `CodeWF.Core` / `CodeWF.Themes` package line into `CodeWF.AvaloniaControls`.
- Removed the obsolete `CodeWF.Core`, `CodeWF.Themes`, and `CodeWF.Themes.StatusBadgeDemo` projects from the solution and scripts.
- Added the `StatusBadge` sample to `CodeWF.AvaloniaControls.Showcase`.

## 12.0.2 - 2026-05-02

- Switched the repository to central NuGet package management with `Directory.Packages.props`.
- Migrated the solution to `.slnx` and pinned the .NET 11 preview SDK with `global.json`.
- Upgraded the repository baseline to .NET 11 and the Avalonia 12 package ecosystem.
- Reorganized the repository into an open source friendly layout: all projects now live physically under `src/`, while sample applications are grouped logically in the `.slnx` solution view.
- Added the `CodeWF.AvaloniaControls.ProDataGrid` package plus the `CodeWF.AvaloniaControls.ProDataGridShowcase` and `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` sample projects.
- Renamed sample applications to clearer open source facing names such as `CodeWF.AvaloniaControls.Showcase`, `CodeWF.AvaloniaControls.ProDataGridShowcase`, `CodeWF.AvaloniaControls.FluentStarterDemo`, and `CodeWF.Themes.StatusBadgeDemo`.
- Added the dedicated `CodeWF.AvaloniaControls.ProDataGridPerformanceDemo` sample for Avalonia 12 large-table tab and document switching verification.
- Added a root `pack.bat` script for one-click restore, build, and NuGet packing.
- Added `publish_all.bat` and `publishbase.bat` for one-click sample publishing.
- Added per-project `CHANGELOG.md` files across the repository.
- Centralized shared pack metadata and root README/CHANGELOG injection through `Directory.Build.props` and `Directory.Build.targets`.

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
