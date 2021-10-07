using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AdeDl.App.Models;

namespace AdeDl.App.Services
{
    public interface IFileManager
    {
        public IEnumerable<T> ReadCsv<T>([NotNull] string path);
    }
}