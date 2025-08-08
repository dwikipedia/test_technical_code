using BookMeetingRoom.Data;
using Microsoft.EntityFrameworkCore;

namespace BookMeetingRoom.API.DbContext
{
    public class BookRoomDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BookRoomDbContext(DbContextOptions<BookRoomDbContext> opt) : base(opt) { }

        public DbSet<Book> Books { get; set; }
    }
}
