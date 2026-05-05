# 更新日志

V12.0.2.1（2026-05-05）

- 😄[新增]-新增根目录 `publish_Markdown.bat`，便于单独发布 Markdown 示例应用
- 😄[新增]-新增 `VC-LTL` 引用，补齐 Windows 发布场景所需运行时依赖
- 🔤[优化]-示例文档改为展示当前 `Markdown` 绑定入口，不再保留旧主题属性写法对照

V12.0.2（2026-05-05）

- 😄[新增]-新增当前工程独立更新日志文件，后续 `CodeWF.Markdown.Sample` 的修改历史改为在工程目录内持续记录
- 😄[新增]-新增 Markdown 示例应用，用于展示基础元素、排版主题、代码块与表格等渲染效果
- 😄[新增]-内置示例文档、示例主题和基础预览界面，方便验证 `CodeWF.Markdown` 与 `CodeWF.Markdown.Themes` 的集成效果
- 😄[新增]-新增 Markdown 示例发布配置、裁剪根和 `net11.0-windows` 目标，并接入根目录 `publish_all.bat` 一键发布脚本
- 🔤[优化]-增量演示改为轮流模拟中文段落替换、正文中部插入和文档尾部追加，避免简单字母串造成不真实的中文 Markdown 测试
- 🔤[优化]-预览区移除对 `MarkdownViewer.BasePath` 的绑定，由 Markdown 文本内容直接驱动渲染
