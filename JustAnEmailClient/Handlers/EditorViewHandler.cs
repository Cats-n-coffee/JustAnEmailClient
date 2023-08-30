using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace JustAnEmailClient.Handlers;

public partial class EditorViewHandler : EditorHandler
{
    public EditorViewHandler() {}
    public EditorViewHandler(IPropertyMapper mapper = null) : base(mapper) {}
}

public partial class EditorViewHandler : EditorHandler
{
    protected override TextBox CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();
        nativeView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        nativeView.Style = null;

        return nativeView;
    }
}
