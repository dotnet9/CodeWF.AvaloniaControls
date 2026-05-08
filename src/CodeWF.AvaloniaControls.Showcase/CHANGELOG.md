# 更新日志

V12.0.2.4 (2026-05-08)

- 🔤[优化]-展示馆主窗体从 `UrsaWindow` 迁移到 `CodeWFWindow`，恢复 Win7 下的托管标题栏拖动，并让 Linux 下控制按钮显示一致
- 🔤[优化]-基础窗体模板已将标题栏与内容区分行布局，删除展示馆主窗体手动顶部偏移
- 😄[新增]-新增标题栏明暗主题切换和运行时语言切换控制
- 😄[新增]-新增控件名称页签标题和控件列表上方的模糊搜索框
- 😄[新增]-接入 `Lang.Avalonia.Json` 本地化，并通过 `I18n/Language.tt` 生成 `zh-CN`、`zh-Hant`、`en-US` 和 `ja-JP` 资源
- 🔤[优化]-本地化展示馆可见文本，以及 `Transfer`、`VComboBox`、`AnimatedImage` 和标记扩展示例中运行时更新的标签
- 🔤[优化]-重整 `TestNoneWindowDemo`：原生 `Window` 负责无边框、可拖动、自定义标题栏和异形窗体示例，`UrsaWindow` 与 `CodeWFWindow` 只保留常规窗体使用示例
- 🔤[优化]-`UrsaWindow` 标准示例改为与 `CodeWFWindow` 一致的常规窗体风格，并删除重复的紧凑工具标题栏示例
- 😄[新增]-新增原生 `Window` 的无边框、可拖动、深色标题栏、高标题栏、圆形、星形和自由形状窗体示例
- 😄[新增]-新增 `UrsaWindow` 强调色标题栏示例，使 Ursa 与 CodeWF 的常规示例集合保持一致
- 🔤[优化]-标准 `CodeWFWindow` 示例改为接近 GitHub Desktop 的窄标题栏风格，控制按钮严格靠右，并增加标准桌面菜单
- 🔤[优化]-修正标题栏菜单下拉项选中、展开、悬浮和按下状态的文字对比度
- 🔤[优化]-移除标题栏控制按钮的鼠标悬浮提示文字
- 🔤[优化]-修正展示馆样式中的 Avalonia Alpha 色值顺序，避免按钮和半透明背景对比度异常
- 🐛[修复]-修复 `TransferDemo` 初始化时机，确保 `MyTransfer` 控件解析后再应用本地化标题

V12.0.2.3（2026-05-06）

- 😄[新增]-新增 `AnimatedImage.Avalonia` 动态 GIF 演示页，展示本地 `Assets/nice.gif` 与网络 GIF 地址加载
- 🔤[优化]-将 `AnimatedImage` 示例入口加入主展示馆左侧页签

V12.0.2（2026-05-02）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.AvaloniaControls.Showcase` 的变更改为在工程目录内持续记录
- 🔤[优化]-保留当前工程作为主控件展示馆示例，并纳入 `.slnx` 的 `samples/showcase` 分组
- 🔤[优化]-对齐仓库当前 Avalonia 12 主线、中央包管理和开源项目目录组织方式，便于持续展示通用控件能力
