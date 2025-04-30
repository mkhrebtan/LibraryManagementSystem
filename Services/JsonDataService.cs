using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;
using System.Text.Json;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Services
{
    public class JsonDataService : IDisposable, ILibraryDataService
    {
        private string _filePath;
        public List<Book> Books { get; private set; }

        ICollection<Book> ILibraryDataService.Books => Books;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
            Books = LoadData();
        }

        public void SetFilePath(string filePath)
        {
            if (filePath.Equals(_filePath))
                return;
            _filePath = filePath;
            Books = LoadData();
        }

        private List<Book> LoadData()
        {
            if (File.Exists(_filePath))
            {
                return JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(_filePath)) ?? new List<Book>();
            }
            else
            {
                throw new FileNotFoundException($"The file {_filePath} does not exist.");
            }
        }

        public void SaveData()
        {
            var json = JsonSerializer.Serialize(Books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void Dispose()
        {
            SaveData();
        }
    }
}