using System;
using Windows.UI.ViewManagement;

namespace _.Services;

/// <summary>
/// Detecta el tema claro/oscuro del sistema Windows y notifica los cambios.
/// </summary>
public class ThemeService
{
    private readonly UISettings _uiSettings = new();

    public bool IsDarkMode { get; private set; }

    public event Action? ThemeChanged;

    public ThemeService()
    {
        IsDarkMode = DetectDarkMode();
        _uiSettings.ColorValuesChanged += (_, _) =>
        {
            var newValue = DetectDarkMode();
            if (newValue != IsDarkMode)
            {
                IsDarkMode = newValue;
                ThemeChanged?.Invoke();
            }
        };
    }

    private bool DetectDarkMode()
    {
        // En modo oscuro el color de fondo del sistema es negro (R=0)
        var bg = _uiSettings.GetColorValue(UIColorType.Background);
        return bg.R == 0;
    }
}
