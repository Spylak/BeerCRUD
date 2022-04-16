using BeerCRUD.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeerCRUD.Data;

public class ApplicationDB :DbContext
{
    public ApplicationDB(DbContextOptions<ApplicationDB> options ) :base(options)
    {
    }

    public DbSet<Beer> Beers { get; set; }
    
}