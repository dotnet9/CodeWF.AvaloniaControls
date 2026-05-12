# Changelog

## 12.0.2.11 - 2026-05-12

- Aligned the default `CodeWFWindow` and `CodeWFTitleBar` theme values with Semi/Ursa window resources such as `WindowDefaultBackground`, `WindowDefaultForeground`, `TitleBarBackground`, and `CaptionButtonForeground`.
- Updated managed caption buttons to use Semi resource keys for size, padding, corner radius, foreground, pointer-over, pressed, disabled, and close-button states.
- Changed the default `CodeWFWindow` title-bar height to 32 so unmanaged and managed title bars start from the same Semi baseline while still allowing per-window overrides.
- Added Avalonia window-decoration element roles to the managed caption buttons for closer parity with Semi and Ursa window templates.

## 12.0.2.10 - 2026-05-11

- Bumped the controls and themes package version to 12.0.2.10 for the `CodeWFWindow` title-bar composition update.
- Added `CodeWFTitleBar` as a standalone title-bar control with `LeftContent`, `CenterContent`, and `RightContent` slots, plus default `Icon + Title` rendering when no custom left content is supplied.
- Refactored the default `CodeWFWindow` template so the title bar overlays the content layer, making cross-title-bar backgrounds and controls natural to compose.
- Added `ContentExtendsIntoTitleBar` to `CodeWFWindow` for layouts that intentionally draw content under the managed title bar.
- Updated the standard `CodeWFWindow` showcase sample with live toggles for default title rendering, custom left content, center content, right content, title-bar margin, and content extension.
- Added a service-console `CodeWFWindow` showcase sample using a shared header mask image and `CodeWF.LogViewer.Avalonia` for the log display.
- Updated central package versions for `CodeWF.LogViewer.Avalonia`, `Lang.Avalonia.Json`, and `YY-Thunks`.

## 12.0.2.9 - 2026-05-09

- Bumped the controls and themes package version to 12.0.2.9 for the `CodeWFWindow` template fix release.
- Restored and named `VisualLayerManager` in the default `CodeWFWindow` template so `ComboBox`, dropdowns, and other popup controls can resolve the overlay host when `OverlayPopups=true`.
- Wired the default `CodeWFWindow` foreground to `SemiGrey9` so regular content text does not inherit unreadable black text in dark-themed surfaces.
- Added Ursa-compatible `CodeWFWindow` title-bar properties for caption button visibility, title-bar visibility, managed resize grips, title-bar content, and title-bar margin.
- Updated `CodeWFWindow` maximize and double-click behavior to respect `CanMaximize` while keeping the Win7/Linux managed title-bar implementation.
- Added interactive controls to the standard `CodeWFWindow` showcase sample so the new title-bar properties can be toggled live.
- Increased default `CodeWFWindow` caption-button hover and pressed contrast so minimize and maximize feedback is easier to see.
- Converted the standard `CodeWFWindow` showcase sample title-bar toggles to ViewModel bindings instead of direct click handlers.
- Fixed the showcase title-bar language selector closed-state layout so the language name and culture tag share one text baseline while the dropdown option layout remains unchanged.
- Centralized library and sample changelog entries in the root English and Chinese changelog files.

## 12.0.2.4 - 2026-05-08

- Moved `CodeWF.AvaloniaControls.ProDataGrid` and its ProDataGrid sample applications to the standalone `CodeWF.AvaloniaControls.ProDataGrid` repository.
- Removed ProDataGrid projects from the `CodeWF.AvaloniaControls` solution, root packing script, sample publish script, and central package version list.
- Moved `CodeWF.AvaloniaControls.Dock` and its Dock sample applications to the standalone `CodeWF.AvaloniaControls.Dock` repository.
- Removed Dock projects from the `CodeWF.AvaloniaControls` solution, root packing script, sample publish script, and central package version list.
- Moved `CodeWF.Markdown`, `CodeWF.Markdown.Themes`, `CodeWF.Markdown.Lite`, `CodeWF.Markdown.Lite.Themes`, and their sample applications to the standalone `CodeWF.Markdown` repository.
- Removed Markdown projects from the `CodeWF.AvaloniaControls` solution, root packing script, and sample publish script.
- Moved the legacy `CodeWF.AvaloniaControls.DataGrid` package and its DataGrid/TreeDataGrid demos to the standalone `CodeWF.AvaloniaControls.DataGrid` repository.
- Updated repository documentation so this repository only describes the Avalonia controls and showcase sample lines.
- Added `CodeWFWindow`, a managed custom window base with title-bar dragging, caption buttons, and resize grips for consistent Win7 and Linux showcase behavior.
- Updated `CodeWFWindow` rounded-corner rendering to use transparent managed chrome instead of native window regions.
- Refined the default `CodeWFWindow` template with an outer frame, inset content surface, consistent right/bottom borders, and smoother small corner radii.
- Added a shared corner-radius inset converter for nested rounded `CodeWFWindow` surfaces.
- Migrated the showcase main window from `UrsaWindow` to `CodeWFWindow` so the content area is laid out below the title bar without manual overlap offsets.
- Added light/dark theme switching, control-name tab search, and JSON-backed dynamic localization to `CodeWF.AvaloniaControls.Showcase`.
- Reworked the showcase window demos so `UrsaWindow` and `CodeWFWindow` focus on regular window styles while borderless, draggable, custom-title, and shaped variants use native `Window`.
- Aligned the standard `UrsaWindow` sample with the regular `CodeWFWindow` sample style and removed duplicate compact tool-title demos.
- Added native `Window` demos for borderless, draggable, dark-title, tall-title, circular, star, and freeform shaped windows.
- Added an `UrsaWindow` accent-title demo to keep the regular Ursa and CodeWF demo sets aligned.
- Updated the standard `CodeWFWindow` sample with a narrow GitHub Desktop-style title bar, right-aligned caption buttons, and standard desktop menus.
- Removed title-bar control button tooltips from `CodeWFWindow` examples.
- Improved title-bar menu selection, open, hover, and pressed states so dropdown text remains readable.
- Improved `CodeWFWindow` caption buttons so the minimize glyph is visible and close hover/press states use the expected red background.
- Corrected Avalonia alpha color ordering in showcase and starter demo styles so translucent badges and window controls render with the intended contrast.

## 12.0.2.3 - 2026-05-06

- Added `CodeWF.Markdown.Lite`, `CodeWF.Markdown.Lite.Themes`, and `CodeWF.Markdown.Lite.Sample` as lightweight Markdown packages and sample app.
- Removed SVG rendering, multi-language resources, CSharpMath, and TextMate dependencies from the Lite Markdown control line; code blocks and math content now render with lightweight text fallbacks.
- Added Win7 AOT compatibility settings to `CodeWF.Markdown.Lite.Sample` with `YY-Thunks`, `VC-LTL`, Windows 6.1 target metadata, and Win7-compatible application manifest entries.
- Added `CodeWF.Markdown.Lite.Sample` to the Markdown publish script and the solution.
- Added an `AnimatedImage.Avalonia` GIF demo to `CodeWF.AvaloniaControls.Showcase` with local and network image sources.

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
