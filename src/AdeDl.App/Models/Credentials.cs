using CsvHelper.Configuration.Attributes;

namespace AdeDl.App.Models
{
    public class Credential
    {
        [Name("username")]
        public string Username { get; set; }
        
        [Name("password")]
        public string Password { get; set; }
        
        [Name("pin")]
        public string Pin { get; set; }
        
        [Name("delega")]
        public string Delega { get; set; }
    }
}