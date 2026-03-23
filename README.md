# LogScreen

Aplicación de escritorio Windows que combina una shell nativa **WinUI 3** con una interfaz web moderna construida sobre **ASP.NET Core Blazor Server**. Está diseñada como un sistema modular de operaciones de negocio: el usuario navega un árbol jerárquico de módulos y cada operación se abre en una ventana independiente.

---

## Características principales

- **Árbol de operaciones modular** — navegación jerárquica por categorías (Maestros, Operaciones, Reportes, Sistema) con búsqueda integrada
- **Multi-ventana nativa** — cada operación se abre en su propia ventana WinUI 3; las ventanas duplicadas se activan en lugar de crearse de nuevo
- **UI web embebida** — la interfaz corre sobre Blazor Server en `localhost:5555`, renderizada mediante WebView2; permite usar componentes web modernos con integración nativa de Windows
- **Material Design** — componentes MudBlazor con tema personalizado (color primario: `#594AE2`)
- **Soporte de tema claro/oscuro** — detección y seguimiento automático del tema del sistema Windows
- **Arquitectura extensible** — diseñada para cargar operaciones desde DLLs externas en el futuro

---

## Arquitectura

```
┌─────────────────────────────────────────────────┐
│                  WinUI 3 Shell                  │
│                                                 │
│  MainWindow ──► WebView2 ──► localhost:5555     │
│                                                 │
│  OperationWindow (×N, una por operación)        │
└─────────────┬───────────────────────────────────┘
              │
┌─────────────▼───────────────────────────────────┐
│          ASP.NET Core + Blazor Server           │
│                                                 │
│  AppLayout (menú + sidebar + árbol de ops)      │
│  LoginLayout (autenticación)                    │
│  OperationPage (contenido de cada operación)    │
└─────────────────────────────────────────────────┘
```

La shell WinUI 3 arranca un servidor web ASP.NET Core en un hilo de fondo. El `MainWindow` y cada `OperationWindow` muestran páginas distintas de esa aplicación web mediante controles WebView2. Esto permite construir la UI con tecnologías web manteniendo integración nativa (ventanas, DPI, tema del sistema).

### Estructura del proyecto

```
LogScreen/
├── App.xaml / App.xaml.cs           # Punto de entrada WinUI
├── MainWindow.xaml / .cs            # Ventana principal, inicia el servidor web
├── Models/
│   └── OperationNode.cs             # Nodo del árbol (id, nombre, ícono, hijos)
├── Services/
│   ├── OperationTreeService.cs      # Construcción y búsqueda del árbol de ops
│   ├── ThemeService.cs              # Detección del tema del sistema Windows
│   └── WindowManager.cs            # Ciclo de vida de ventanas de operación
├── Views/
│   ├── MyWebview.cs                 # Control WebView2 personalizado
│   └── OperationWindow.xaml / .cs  # Ventana dinámica por operación
├── Components/                      # UI Blazor
│   ├── Pages/
│   │   ├── Home.razor               # Página de login
│   │   └── OperationPage.razor      # Página de operación dinámica
│   └── Layout/
│       ├── AppLayout.razor          # Layout principal con menú y sidebar
│       └── LoginLayout.razor        # Layout de autenticación
└── Utils/
    └── Helper.cs                    # Utilidades: file picker, pantalla completa, encoding
```

---

## Tecnologías

| Categoría | Tecnología |
|---|---|
| Plataforma | .NET 8, Windows 10+ (x64) |
| UI nativa | WinUI 3 / Windows App SDK 1.5 |
| UI web | ASP.NET Core + Blazor Server |
| Componentes | MudBlazor 6 (Material Design) |
| Renderizado | Microsoft WebView2 (Chromium) |
| Base de datos | LiteDB 5 (embebida) |
| MVVM | CommunityToolkit.Mvvm 8 |
| Serialización | Newtonsoft.Json |
| Parsing HTML | AngleSharp |
| Encoding | UTF.Unknown |

---

## Requisitos

- Windows 10 versión 1809 (build 17763) o superior
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Windows App SDK 1.5](https://learn.microsoft.com/windows/apps/windows-app-sdk/downloads)
- Visual Studio 2022 17.8+ con carga de trabajo **Desarrollo de aplicaciones de Windows** (o VS Code con extensión C#)

---

## Compilar y ejecutar

```bash
# Restaurar dependencias y compilar
dotnet build LogScreen.sln -p:Platform=x64

# Ejecutar en modo Debug
dotnet run --project LogScreen.csproj
```

> El proyecto es **self-contained** (`WindowsAppSDKSelfContained=true`), por lo que no requiere instalación separada del runtime de Windows App SDK en la máquina destino al publicar.

### Publicar

```bash
dotnet publish LogScreen.csproj -c Release -p:Platform=x64
```

---

## Flujo de la aplicación

1. **Inicio** — `App.xaml.cs` crea el `MainWindow`, que lanza el servidor Blazor en segundo plano en el puerto `5555`
2. **Login** — el `WebView2` carga `http://127.0.0.1:5555` y presenta la pantalla de login (credenciales por defecto: `admin` / `1234`); la cuenta se bloquea tras 4 intentos fallidos
3. **Navegación** — tras autenticarse, el usuario ve el árbol de operaciones en el sidebar izquierdo con búsqueda y controles de expansión
4. **Abrir operación** — al seleccionar un nodo hoja, `WindowManager` crea un `OperationWindow` que carga `http://127.0.0.1:5555/operation/{id}`; si ya existe esa ventana, la activa
5. **Multi-ventana** — el contador de ventanas abiertas se muestra en el sidebar en tiempo real

---

## Módulos de operación por defecto

| Categoría | Operaciones |
|---|---|
| Maestros | Clientes, Proveedores, Productos |
| Operaciones | Pedidos, Facturas, Pagos |
| Reportes | Ventas, Stock, Auditoría |
| Sistema | Usuarios, Permisos, Configuración |

> Estos módulos son el árbol por defecto. La arquitectura está preparada para cargar operaciones dinámicamente desde DLLs externas.

---

## Notas de desarrollo

- **Threading**: el servidor web corre en un `Task` de larga duración; las operaciones sobre ventanas WinUI se ejecutan en el `DispatcherQueue` del hilo UI.
- **DPI**: el manifiesto configura `PerMonitorV2` para escala correcta en monitores de alta resolución.
- **Tema**: `ThemeService` suscribe al evento `ColorValuesChanged` de `UISettings` para reaccionar a cambios de tema en caliente.
- **WebView2**: `MyWebview` intercepta la navegación para evitar popups y gestiona el modo pantalla completa para vídeo.
