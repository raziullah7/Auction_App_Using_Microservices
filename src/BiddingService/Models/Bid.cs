using MongoDB.Entities;

namespace BiddingService.Models;

public class Bid : Entity
{
    public string AuctionId { get; set; }
    public string Bidder { get; set; }
    public DateTime BidTime { get; set; }
    public int Amount { get; set; }
    public BidStatus BidStatus { get; set; }
}