using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities
{
    // use the following command to tell the Entity Framework to use the
    // given name for the table during Code First Migration
    [Table("Items")]
    public class Item
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; }

        // nav properties to establish one-to-one relationship with Auction.cs
        // using the conventional method (which is nav properties)
        public Auction Auction { get; set; }
        public Guid AuctionId { get; set; }

    }
}
