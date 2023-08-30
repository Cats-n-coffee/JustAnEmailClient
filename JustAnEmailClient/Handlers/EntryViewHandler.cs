using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Notifications;

namespace JustAnEmailClient.Handlers;

public partial class EntryViewHandler : EntryHandler
{
    public EntryViewHandler() {}
    public EntryViewHandler(IPropertyMapper mapper = null) : base(mapper) {}
}

public partial class EntryViewHandler : EntryHandler
{
    protected override TextBox CreatePlatformView()
    {
        var nativeView = base.CreatePlatformView();
        nativeView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        nativeView.Style = null;

        return nativeView;
    }
}
