using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Interfaces
{
    interface IBookAvailabilityObserver
    {
        void NotifyBookAvailable(int bookId, string bookTitle);
    }
}
