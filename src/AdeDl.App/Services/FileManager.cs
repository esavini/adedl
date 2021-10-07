using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdeDl.App.Services
{
    public class FileManager : IFileManager
    {
        public IEnumerable<T> ReadCsv<T>([NotNull] string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);

            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, config);

            return csv.GetRecords<T>().ToList();
        }
    }
}