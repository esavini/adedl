using System.ComponentModel;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;

namespace AdeDl.App.Models
{
    public class FiscalEntity : INotifyPropertyChanged
    {
        private string _notes;

        [Name("denominazione")]
        public string Name { get; set; }

        [Name("cf")]
        public string FiscalCode { get; set; }

        [Name("f24")]
        public int? F24Year { get; set; }

        [Name("cu")]
        public int? CuYear { get; set; }

        [Ignore]
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}