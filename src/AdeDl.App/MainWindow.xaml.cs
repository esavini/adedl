using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using AdeDl.App.Services;
using AdeDl.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;

namespace AdeDl.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Browser _browser;

        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = ServiceLocator.Services.GetService<MainWindowViewModel>();
            DataContext = _viewModel;
            InitializeComponent();
            
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docsPath, "AdeDl", "utenti.csv");

            if (File.Exists(path)) return;
            
            using var file = File.CreateText(path);
            file.WriteLine(@"username,password,pin,delega");
            file.Close();
        }
    }
}