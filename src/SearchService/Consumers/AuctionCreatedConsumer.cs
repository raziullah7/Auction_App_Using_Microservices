using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    // dependency injection of AutoMapper
    private readonly IMapper _mapper;
    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    // what happens when a "AuctionCreated" object is consumed
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming auction created " + context.Message.Id);

        // getting <Item> from (context.Message)
        var item = _mapper.Map<Item>(context.Message);
        
        // saving changes
        await item.SaveAsync();
    }
}