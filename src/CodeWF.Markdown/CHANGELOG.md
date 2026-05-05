# 更新日志

V12.0.2.1（2026-05-05）

- 🔤[优化]-移除 `MarkdownViewer.Value` 兼容入口，`MarkdownViewer` 只通过 `Markdown` 属性接收原文
- 🔤[优化]-补充 `MarkdownViewer`、代码块工具栏事件参数、样式 Key 和图片路径相关公开 API 注释
- 🔤[优化]-补充 Markdown 控件使用文档，覆盖样式注册、XAML 绑定、主题切换和图片路径处理

V12.0.2（2026-05-05）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.Markdown` 的修改历史改为在工程目录内持续记录
- 😄[新增]-新增基于 Avalonia 12 的 Markdown 渲染控件，支持常规 Markdown 元素、代码块渲染、图片预览与增量刷新
- 😄[新增]-接入 Markdig、TextMateSharp 与内置样式 Key，作为 `CodeWF.Markdown.Themes` 和 Markdown 示例工程的基础渲染库
- 🔤[优化]-移除 `MarkdownViewer.BasePath`，`MarkdownViewer` 不再对外暴露文件路径配置，保持以 Markdown 文本输入为主的控件接口
- 🔤[优化]-新增 `DocumentBottomPadding` 和底部占位样式 Key，用于改善滚动到底时的文档尾部留白
