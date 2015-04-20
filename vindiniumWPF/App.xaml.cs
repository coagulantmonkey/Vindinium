using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using vindiniumWPF.View_Models;
using vindiniumWPF.Views;

namespace vindiniumWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow view = new MainWindow();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            view.DataContext = viewModel;
            view.Show();
        }
    }
}
