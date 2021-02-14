using CoronaTest.Wpf.Common;
using CoronaTest.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CoronaTest.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var controller = new WindowController();
            var viewModel = await MainViewModel.Create(controller);
            controller.ShowWindow(viewModel);
        }
    }
}
