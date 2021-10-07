using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using AdeDl.App.Commands;
using AdeDl.App.Exceptions;
using AdeDl.App.Models;
using AdeDl.App.Services;
using CsvHelper;
using Microsoft.Win32;

namespace AdeDl.App.ViewModels
{
    /// <summary>
    /// MainWindow's view model
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IFileManager _fileManager;
        private readonly ILoginService _loginService;
        private readonly ICuService _cuService;
        private readonly IF24Service _f24Service;

        public MainWindowViewModel(IFileManager fileManager, ILoginService loginService, ICuService cuService,
            IF24Service f24Service)
        {
            _fileManager = fileManager;
            _loginService = loginService;
            _cuService = cuService;
            _f24Service = f24Service;
            Loading = Visibility.Hidden;
            try
            {
                var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var path = System.IO.Path.Combine(docsPath, "AdeDl", "utenti.csv");

                var entities = _fileManager.ReadCsv<Credential>(path);

                Deleghe.Clear();

                foreach (var entity in entities)
                    Deleghe.Add(entity);
            }
            catch
            {
                //
            }
        }

        private Visibility _loading;

        private string _path;

        private string _pin;

        private bool _loginEnabled = true;

        private string _fiscalCode;

        private double _percentage;

        private bool _indeterminate;
        
        private Credential _delega;
        
        private bool _disabledStartButton;

        public ObservableCollection<Credential> Deleghe { get; } = new();

        public Credential Delega
        {
            get => _delega;
            set
            {
                _delega = value;
                OnPropertyChanged();
            }
        }

        public Visibility Loading
        {
            get => _loading;
            set
            {
                _loading = value;
                OnPropertyChanged();
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public string Pin
        {
            get => _pin;
            set
            {
                _pin = value;
                OnPropertyChanged();
            }
        }

        public bool LoginEnabled
        {
            get => _loginEnabled;
            set
            {
                _loginEnabled = value;
                OnPropertyChanged();
            }
        }

        public string FiscalCode
        {
            get => _fiscalCode;
            set
            {
                _fiscalCode = value;
                OnPropertyChanged();
            }
        }

        public double Percentage
        {
            get => _percentage;
            set
            {
                _percentage = value;
                OnPropertyChanged();
            }
        }

        public bool Indeterminate
        {
            get => _indeterminate;
            set
            {
                _indeterminate = value;
                OnPropertyChanged();
            }
        }

        public ICommand FileSelectionCommand => new Command(FileSelection);

        public ICommand StartCommand => new Command(Start);

        public ObservableCollection<FiscalEntity> Entities { get; } = new();

        private async void Start()
        {
            if (_disabledStartButton) return;

            _disabledStartButton = true;

            if (Delega is null) return;
            
            Loading = Visibility.Visible;
            Indeterminate = true;
            Percentage = 0;

            await _loginService.LoginAsync(Delega);

            Indeterminate = false;

            int counter = 0;
            foreach (var entity in Entities)
            {
                entity.Notes = "";

                try
                {
                    entity.Notes += "CU: ";

                    if (entity.CuYear.HasValue)
                    {
                        await _cuService.DownloadCuAsync(entity, _loginService.Cookies, Delega.Delega);
                        entity.Notes += "OK; ";
                    }
                    else
                    {
                        entity.Notes += "saltato; ";
                    }
                }
                catch (NotAvailableException)
                {
                    entity.Notes += "Annualità non disponibile; ";
                }
                catch
                {
                    entity.Notes += "ERRORE; ";
                }
                finally
                {
                    counter++;
                    Percentage = 100.0 * counter / (Entities.Count * 2);
                }

                try
                {
                    entity.Notes += "F24: ";

                    if (entity.F24Year.HasValue)
                    {
                        await _f24Service.DownloadF24Async(entity, _loginService.Cookies, Delega.Delega);
                        entity.Notes += "OK; ";
                    }
                    else
                    {
                        entity.Notes += "saltato; ";
                    }
                }
                catch (NotAvailableException)
                {
                    entity.Notes += "Annualità non disponibile; ";
                }
                catch
                {
                    entity.Notes += "ERRORE; ";
                }
                finally
                {
                    counter++;
                    Percentage = 100.0 * counter / (Entities.Count * 2);
                }
            }

            Loading = Visibility.Hidden;
            _disabledStartButton = false;
        }

        private void FileSelection()
        {
            var fileDialog = new OpenFileDialog {Filter = "File CSV|*.csv|All files|*.*"};

            var fileSelected = fileDialog.ShowDialog() ?? false;

            if (fileSelected)
                Path = fileDialog.FileName;

            else
                return;

            try
            {
                var entities = _fileManager.ReadCsv<FiscalEntity>(Path);

                Entities.Clear();

                foreach (var entity in entities)
                    Entities.Add(entity);
            }
            catch (IOException)
            {
                MessageBox.Show("Impossibile leggere il file CSV.");
            }
            catch (HeaderValidationException)
            {
                MessageBox.Show("Intestazione del file non valida.");
            }
        }
    }
}