using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.Interfaces
{
    interface IBookAvailabilityPublisher
    {
        void Subscribe(int bookId, int userId);
        void Unsubscribe(int bookId, int userId);
        void NotifySubscribers(int bookId);
    }
}
