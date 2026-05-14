using CodeWF.AvaloniaControls.Controls;
using CodeWF.Log.Core;

namespace CodeWF.AvaloniaControlsDemo.Views.TestNoneWindowDemos;

public partial class CodeWFWindowServiceConsoleDemo : CodeWFWindow
{
    private bool _seededLogs;

    public CodeWFWindowServiceConsoleDemo()
    {
        InitializeComponent();
        Loaded += (_, _) => SeedLogs();
    }

    private void SeedLogs()
    {
        if (_seededLogs)
        {
            return;
        }

        _seededLogs = true;
        Logger.EnableConsoleOutput = false;

        Logger.InfoToUI("注册钩挂通过");
        Logger.InfoToUI("消息服务启动完成");
        Logger.InfoToUI("控制服务启动完成");
        Logger.InfoToUI("所有服务启动成功");
        Logger.InfoToUI("数据库消息服务从机的恢复");
        Logger.ErrorToUI("new client ->127.0.0.1:50802");
        Logger.ErrorToUI("收到配置消息获取请求");
        Logger.InfoToUI("收到工况列表请求");
        Logger.ErrorToUI("new client ->127.0.0.1:50816");
        Logger.ErrorToUI("收到配置消息获取请求");
        Logger.InfoToUI("收到工况列表请求");
        Logger.InfoToUI("ReceiveCallback Error远程主机强迫关闭了一个现有的连接");
    }
}
