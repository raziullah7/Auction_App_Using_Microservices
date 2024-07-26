using Contracts;
using MassTransit;
using MongoDB.Entities;
using Item = SearchService.Models.Item;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> Consuming auction finished");
        
        // getting the newly created auction in PostGres
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
        
        // if item was sold, update Winner name and SoldAmount
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount;
        }
        
        // setting the status
        auction.Status = "Finished";
        
        // save changes
        await auction.SaveAsync();
    }
}