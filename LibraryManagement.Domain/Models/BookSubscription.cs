using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Models
{
    public class BookSubscription
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime SubscribedAt { get; set; }
        public bool IsNotified { get; set; } = false;

        public Book Book { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
