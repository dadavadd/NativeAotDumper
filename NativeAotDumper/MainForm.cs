using NativeAotDumper.Core.NativeAot;
using System.Globalization;

namespace NativeAotDumper;

public partial class MainForm : Form
{
    private readonly Dumper _parser;
    public MainForm()
    {
        InitializeComponent();

        _parser = new("NativeHeller");
        _parser.Run();

        foreach (var (address, name) in _parser.TypeNames)
        {
            listBox1.Items.Add($"Address: {address,-5:X} | {name}");
        }

        foreach (var frozenString in _parser.FrozenStrings)
        {
            listBox2.Items.Add($"Address: {frozenString.StartAddress,-5:X} | String: {frozenString.Data}");
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _parser.Dispose();

        base.OnFormClosed(e);
    }
}
