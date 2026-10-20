using Bolocan_Izabela_Lab2.Models;
using Microsoft.EntityFrameworkCore;

public class Bolocan_Izabela_Lab2Context(DbContextOptions<Bolocan_Izabela_Lab2Context> options) : DbContext(options)
{
    public DbSet<Bolocan_Izabela_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Bolocan_Izabela_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Bolocan_Izabela_Lab2.Models.Author> Authors { get; set; } = default!;
    public DbSet<Category> Category { get; set; }
}
