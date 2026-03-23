using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using _.Components;
using _.Services;
using MudBlazor.Services;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace _
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent()
            (this.AppWindow.Presenter as Microsoft.UI.Windowing.OverlappedPresenter)?.Maximize();
            var task=new Task(()=> {
                var builder = WebApplication.CreateBuilder();

                // Add services to the container.
                builder.Services.AddRazorComponents().AddInteractiveServerComponents();
                builder.Services.AddMudServices();
                builder.Services.AddSingleton(new WindowManager(DispatcherQueue.GetForCurrentThread()));
                builder.Services.AddSingleton<OperationTreeService>();
                builder.Services.AddSingleton<ThemeService>();

                builder.WebHost.UseUrls("http://*:5555");
                var app = builder.Build();

                // Configure the HTTP request pipeline.


                app.UseStaticFiles();
                
                app.UseRouting();
                app.UseAntiforgery();
                app.MapRazorComponents<_.Components.App>().AddInteractiveServerRenderMode();
                app.Run();

            }, TaskCreationOptions.LongRunning);
            task.Start();
        }

        
    }
}
