# 更新日志

V12.0.2（2026-05-05）

- 😄[新增]-合并原 `CodeWF` 工程中的 `StatusBadge` 控件，统一纳入 `CodeWF.AvaloniaControls` 主控件库维护
- 🔤[优化]-将 `StatusLabel`、`StatusCard`、`SearchListBox`、`Transfer` 改造为模板化控件，核心库不再承载实际 XAML 模板
- 🔤[优化]-核心库精简为控件 API、状态模型、Helper、扩展和绘制逻辑，实际视觉模板迁移到 `CodeWF.AvaloniaControls.Themes`

V12.0.2（2026-05-02）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.AvaloniaControls` 主控件库改动将在工程目录内持续记录
- 🔤[优化]-对齐仓库当前 `.NET 11 + Avalonia 12` 主线，并纳入中央包管理、`.slnx` 解决方案和统一打包流程
- 🔤[优化]-将当前工程整理为开源分发基线，继续作为通用控件库主包参与整仓构建和 NuGet 输出
