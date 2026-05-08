# 更新日志

V12.0.2.4 (2026-05-08)

- Added the default `CodeWFWindow` control theme with a 60px managed title bar, right-aligned caption buttons, maximize/restore glyph switching, and resize hit areas.
- Improved the caption button theme with a visible minimize glyph, lightweight hover states, and red close hover/pressed backgrounds.
- Added a default outer border to `CodeWFWindow` so custom-decorated windows remain visually separated from their parent background.
- Refined the default `CodeWFWindow` caption buttons to use larger square system-style hit targets.

V12.0.2（2026-05-05）

- 😄[新增]-新增 `CodeWF.AvaloniaControls.Themes` 主题工程，集中承载 `CodeWF.AvaloniaControls` 的控件模板、转换器和视觉资源
- 🔤[优化]-将主控件库改造为核心控件与渲染逻辑包，实际 XAML 模板和主题资源迁移到当前主题包
- 🔤[优化]-合并原 `CodeWF.Themes` 中的 `StatusBadge` 模板与明暗主题资源，统一由 `CodeWF.AvaloniaControls.Themes` 维护
