# 更新日志

V12.0.2.4 (2026-05-08)

- 😄[新增]-新增 `CodeWFWindow` 托管 Avalonia 窗体基类，提供自定义标题栏拖动、控制按钮和缩放热区，统一 Win7 与 Linux 下的窗体行为
- 😄[新增]-新增标题栏左右内容槽和标题栏样式属性，应用可直接放置品牌、菜单和操作区，不必依赖 `UrsaWindow`
- 🔤[优化]-改进 `CodeWFWindow` 最大化/还原状态、控制按钮启用状态和缩放热区显隐处理
- 🔤[优化]-`CodeWFWindow` 改用透明托管窗体外壳渲染圆角，移除原生窗口区域裁剪逻辑，避免小圆角锯齿感和边框突兀问题

V12.0.2（2026-05-05）

- 😄[新增]-合并原 `CodeWF` 工程中的 `StatusBadge` 控件，统一纳入 `CodeWF.AvaloniaControls` 主控件库维护
- 🔤[优化]-将 `StatusLabel`、`StatusCard`、`SearchListBox`、`Transfer` 改造为模板化控件，核心库不再承载实际 XAML 模板
- 🔤[优化]-核心库精简为控件 API、状态模型、Helper、扩展和绘制逻辑，实际视觉模板迁移到 `CodeWF.AvaloniaControls.Themes`

V12.0.2（2026-05-02）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.AvaloniaControls` 主控件库改动将在工程目录内持续记录
- 🔤[优化]-对齐仓库当前 `.NET 11 + Avalonia 12` 主线，并纳入中央包管理、`.slnx` 解决方案和统一打包流程
- 🔤[优化]-将当前工程整理为开源分发基线，继续作为通用控件库主包参与整仓构建和 NuGet 输出
