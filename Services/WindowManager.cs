using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using _.Models;

namespace _.Services;

/// <summary>
/// Abre ventanas de operación en el hilo de UI de WinUI.
/// La lógica de negocio de cada operación corre en su propio thread.
/// </summary>
public class WindowManager
{
    private readonly DispatcherQueue _dispatcher;
    private readonly List<Window> _openWindows = [];

    public WindowManager(DispatcherQueue dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public void OpenOperation(OperationNode operation)
    {
        // Las ventanas WinUI deben crearse en el hilo de UI
        _dispatcher.TryEnqueue(() =>
        {
            // Si ya está abierta, la trae al frente
            var existing = _openWindows
                .OfType<Views.OperationWindow>()
                .FirstOrDefault(w => w.OperationId == operation.Id);

            if (existing is not null)
            {
                existing.Activate();
                return;
            }

            var window = new Views.OperationWindow(operation);
            window.Closed += (s, e) => _openWindows.Remove(window);
            _openWindows.Add(window);
            window.Activate();
        });
    }

    public int OpenWindowCount => _openWindows.Count;
}
