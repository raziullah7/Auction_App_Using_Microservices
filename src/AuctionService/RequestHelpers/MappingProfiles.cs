using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Auction to AuctionDto
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);

            // Item to AuctionDto
            CreateMap<Item, AuctionDto>();

            // CreateAuctionDto to Auction
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(dest => dest.Item, 
                    opt => opt.MapFrom(src => src));

            // CreateAuctionDto to Item
            CreateMap<CreateAuctionDto, Item>();
            
            // AuctionDto to AuctionCreated
            CreateMap<AuctionDto, AuctionCreated>();
            
            // AuctionDto to AuctionUpdated
            CreateMap<Auction, AuctionUpdated>().IncludeMembers(a => a.Item);
            
            // AuctionDto to AuctionDeleted
            CreateMap<Item, AuctionUpdated>();
        }
    }
}
