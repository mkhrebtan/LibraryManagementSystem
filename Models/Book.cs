using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        #region Properties
        private int _id;
        public required int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("ID cannot be negative.");
                _id = value;
            }
        }

        private string _title = "";
        public required string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Title cannot be null or empty.");
                _title = value;
            }
        }

        private string _genre = "";
        public required string Genre
        {
            get => _genre;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Genre cannot be null or empty.");
                _genre = value;
            }
        }

        private string _author = "";
        public required string Author
        {
            get => _author;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Author cannot be null or empty.");
                _author = value;
            }
        }

        private int _publicationYear;
        public required int PublicationYear
        {
            get => _publicationYear;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Publication year cannot be negative.");
                if (value > DateTime.Now.Year)
                    throw new ArgumentException("Publication year cannot be in the future.");
                _publicationYear = value;
            }
        }

        private int _pageCount;
        public required int PageCount
        {
            get => _pageCount;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Page count must be positive.");
                _pageCount = value;
            }
        }

        private bool _isAvailable = true;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => _isAvailable = value;
        }
        
        #endregion       

        public delegate void BookReturnedHandler(Book book);

        private BookReturnedHandler? _bookReturnedHandlers;

        public void AddBookReturnedHandler(BookReturnedHandler handler)
        {
            if (_isAvailable)
            {
                throw new InvalidOperationException("Book is already available.");
            }
            _bookReturnedHandlers += handler;
        }

        public void RemoveBookReturnedHandler(BookReturnedHandler handler)
        {
            _bookReturnedHandlers -= handler;
        }

        public void Reserve()
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Book is already reserved.");
            IsAvailable = false;
        }

        public void Return()
        {
            if (IsAvailable)
                throw new InvalidOperationException("Book is already available.");
            IsAvailable = true;
            _bookReturnedHandlers?.Invoke(this);
        }

        public override string ToString()
        {
           var asciiArt = @$" 
    _______
   /      /, ""{_title}"" ({_id})
  /      //  By {_author}, {_publicationYear}
 /______//   {_pageCount} pages
(______(/    Genre: {_genre}
Available: {(_isAvailable ? "Yes" : "No")}";
            return asciiArt;
        }
    }
}