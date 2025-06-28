using NativeAotDumper.Core.NativeAot;

namespace NativeAotDumper;

public partial class MainForm : Form
{
    private readonly Dumper _parser;

    public MainForm()
    {
        InitializeComponent();

        _parser = new("DirectCallInjector");


        foreach (var frozenString in _parser.FrozenStrings)
        {
            listBox2.Items.Add($"Address: 0x{frozenString.StartAddress:X} | String: {frozenString.Data}");
        }

        foreach (var typeInfo in _parser.RuntimeTypeInfos)
        {
            listBox1.Items.Add($"[Type] {typeInfo.FullName} MethodTable 0x{typeInfo.MethodTableAddress:X}");

            if (typeInfo.Fields.Count > 0)
            {
                listBox1.Items.Add($"   └─ Fields ({typeInfo.Fields.Count}):");
                foreach (var field in typeInfo.Fields)
                {
                    listBox1.Items.Add($"       • {field.Name} | Token: 0x{field.MetadataToken:X8} | Offset: 0x{field.Offset:X}");
                }
            }

            if (typeInfo.Properties.Count > 0)
            {
                listBox1.Items.Add($"   └─ Properties ({typeInfo.Properties.Count}):");
                foreach (var prop in typeInfo.Properties)
                {
                    listBox1.Items.Add($"       • {prop.Name} | Token: 0x{prop.MetadataToken:X8}");
                }
            }

            if (typeInfo.Methods.Count > 0)
            {
                listBox1.Items.Add($"   └─ Methods ({typeInfo.Methods.Count}):");
                foreach (var method in typeInfo.Methods)
                {
                    listBox1.Items.Add($"       • {method.Name} | Token: 0x{method.MetadataToken:X8}");
                }
            }

            listBox1.Items.Add(string.Empty);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _parser.Dispose();
        base.OnFormClosed(e);
    }
}
