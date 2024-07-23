using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext _dbContext;
    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> Consuming auction finished");
        
        // getting the newly created auction in PostGres
        var auction = await _dbContext.Auctions.FindAsync(context.Message.AuctionId);
        
        // if item was sold, update Winner name and SoldAmount
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }
        
        // setting the status
        auction.Status = auction.SoldAmount > auction.ReservePrice
            ? Status.Finished
            : Status.ReserveNotMet;
        
        // save changes
        await _dbContext.SaveChangesAsync();
    }
}