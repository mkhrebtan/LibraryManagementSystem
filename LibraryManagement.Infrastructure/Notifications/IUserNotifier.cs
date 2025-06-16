using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.BookAvailability;

namespace LibraryManagement.Infrastructure.Notifications
{
    internal interface IUserNotifier : IBookAvailabilityObserver
    {

    }
}
