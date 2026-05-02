# 更新日志

V12.0.2（2026-05-02）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.AvaloniaControls.DataGrid` 的变更改为在工程目录内持续记录
- 😄[新增]-保留最后一个免费开源官方 `Avalonia.Controls.DataGrid` 与 `Avalonia.Controls.TreeDataGrid` 兼容链路，便于继续对外分发旧版扩展包
- 🔤[优化]-将当前工程改为显式固定旧版兼容依赖，不再走中央包管理，避免与 Avalonia 12 主线示例产生版本牵制
- 🔤[优化]-整理并保留 `DataGrid` 三态排序、智能提示，以及 `TreeDataGrid` 三态排序与全选扩展，方便示例和业务项目复用
