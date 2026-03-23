using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using _.Models;

namespace _.Views;

public sealed partial class OperationWindow : Window
{
    public string OperationId { get; }

    public OperationWindow(OperationNode operation)
    {
        InitializeComponent();
        OperationId = operation.Id;
        Title = operation.Name;

        // Tamaño inicial de la ventana de operación
        AppWindow.Resize(new Windows.Graphics.SizeInt32(1024, 720));

        webView.Source = new Uri($"http://127.0.0.1:5555/operation/{Uri.EscapeDataString(operation.Id)}");
    }
}
