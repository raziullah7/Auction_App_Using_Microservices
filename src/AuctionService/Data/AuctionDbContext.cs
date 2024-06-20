using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    //////// new way of writing default constructor ////////
    public class AuctionDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Auction> Auctions { get; set; }
    }

    //////// traditional way of writing default constructor ////////
    // public class AuctionDbContext : DbContext
    // {
    //     public AuctionDbContext(DbContextOptions options) : base(options)
    //     {
    //     }

    //     public DbSet<Auction> Auctions { get; set; }
    // }
}
