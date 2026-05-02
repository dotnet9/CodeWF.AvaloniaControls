using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CodeWF.AvaloniaControls.Extensions;
using System.ComponentModel;

namespace CodeWF.AvaloniaControls.Showcase.Pages;

public partial class TransferDemo : UserControl, INotifyPropertyChanged
{
    private string? _selectedInfo;

    public TransferDemo()
    {
        InitializeComponent();
        RightItems.CollectionChanged += (s, e) =>
        {
            UpdateSelectedCount();
        };
    }

    public RangeObservableCollection<string> LeftItems { get; set; } = ["codewf.com", "dotnet9.com", "dotnetools.com", "dotnet.chat", "Ding", "Otter",
    "������ Avalonia DataGrid һ�����͵�","�����ݸ߶ȳ�����������","�����п�֮�͵��ж��߼���","��ͨ���Զ�����ʽ","rid �Ĳ�������Ԥ���ֱ��������",
        "�� DataGrid ����С��ȸ�����","���û�б������п�ļ���","���п�֮�͵��ж��߼�����","Grid �����ScrollViewer"];

    public RangeObservableCollection<string> RightItems { get; set; } = ["Husky", "Mr.17", "Cass", "�Զ��� DataGrid ��ʽ������", "�������ScrollViewer",
        "����ͨ���޸� DataGrid ��Ĭ����ʽ", "ǿ���䲼��ʱԤ���ֱ�������Ŀ��", "ȷ��ˮƽ��������ȷ����", "DataGrid ��ʼ��Ԥ���ֱ�������Ŀ��", "���������������٣�ˮƽ��������������п�"];

    public string? SelectedInfo
    {
        get => _selectedInfo;
        set
        {
            if (_selectedInfo == value) return;
            _selectedInfo = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedInfo)));
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
        SelectedInfo = RightItems.Count <= 0 ? "��" : string.Join(',', RightItems);
    }
}