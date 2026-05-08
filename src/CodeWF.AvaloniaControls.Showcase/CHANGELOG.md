# 更新日志

V12.0.2.4 (2026-05-08)

- Migrated the showcase main window from `UrsaWindow` to `CodeWFWindow` to restore managed title-bar dragging on Win7 and show caption buttons consistently on Linux.
- Removed the manual top content offset now that the base window template lays out the title bar and content area in separate rows.
- Added title-bar controls for light/dark theme switching and runtime language switching.
- Added control-name tab headers and a fuzzy search box above the control list.
- Added `Lang.Avalonia.Json` localization with `zh-CN`, `zh-Hant`, `en-US`, and `ja-JP` resources generated through `I18n/Language.tt`.
- Localized the visible showcase text and runtime-updated labels used by `Transfer`, `VComboBox`, `AnimatedImage`, and markup extension demos.
- Expanded `TestNoneWindowDemo` with native `Window`, `UrsaWindow`, and `CodeWFWindow` samples including borderless, draggable, compact, tall-title, dark-title, accent-title, and shaped-window variants.
- Fixed `TransferDemo` initialization so localized headers are applied after the `MyTransfer` control is resolved.

V12.0.2.3（2026-05-06）

- 😄[新增]-新增 `AnimatedImage.Avalonia` 动态 GIF 演示页，展示本地 `Assets/nice.gif` 与网络 GIF 地址加载
- 🔤[优化]-将 `AnimatedImage` 示例入口加入主展示馆左侧页签

V12.0.2（2026-05-02）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.AvaloniaControls.Showcase` 的变更改为在工程目录内持续记录
- 🔤[优化]-保留当前工程作为主控件展示馆示例，并纳入 `.slnx` 的 `samples/showcase` 分组
- 🔤[优化]-对齐仓库当前 Avalonia 12 主线、中央包管理和开源项目目录组织方式，便于持续展示通用控件能力
