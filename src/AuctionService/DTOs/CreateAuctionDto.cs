using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs
{
    // contains the details of the item to be put on auction
    public class CreateAuctionDto
    {
        // from Item.cs
        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public int Mileage { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        // from Auction.cs
        [Required]
        public int ReservePrice { get; set; }

        [Required]
        public DateTime AuctionEnd { get; set; }
    }
}
