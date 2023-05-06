using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Data
{
    public class ContactAPIDbContext : DbContext
    {
        public ContactAPIDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ContactModel> Contacts { get; set; }
    }
}
