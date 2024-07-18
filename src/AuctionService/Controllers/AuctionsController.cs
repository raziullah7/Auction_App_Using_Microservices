using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
        // services needed as Dependency Injection
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        // making Dependency Injection of 2 services i.e., DbContext and AutoMapper
        public AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        //---------------------------------- Endpoint # 1 ----------------------------------
        [HttpGet]   // GET all auctions from the DB
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            var query = _context.Auctions
                .OrderBy(x => x.Item.Make)
                .AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt
                    .CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        //---------------------------------- Endpoint # 2 ----------------------------------
        [HttpGet("{id}")]    // GET auction by Id from the DB
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (auction == null) return NotFound();

            return _mapper.Map<AuctionDto>(auction);
        }

        //---------------------------------- Endpoint # 3 ----------------------------------
        [Authorize]
        [HttpPost]  // POST an auction to the DB
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            
            // add current user as seller
            auction.Seller = User.Identity.Name;

            // adding to the DB
            _context.Auctions.Add(auction);
            
            // publishing an Event for Consumers to consume
            var newAuction = _mapper.Map<AuctionDto>(auction);
            // _mapper takes an AuctionDto object and converts it into AuctionCreated object
            // the AuctionCreated object is then published by _publishEndpoint to all Consumers
            await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

            // saving changes if the query was successful
            // (result is only assigned if the number of changes are greater than zero)
            var result = await _context.SaveChangesAsync() > 0;

            // if the request was not successful
            if (!result) return BadRequest("Could not save changes to the DB.");

            // return the reference 
            return CreatedAtAction(nameof(GetAuctionById), 
                new { auction.Id }, _mapper.Map<AuctionDto>(auction));
        }

        //---------------------------------- Endpoint # 4 ----------------------------------
        [Authorize]
        [HttpPut("{id}")]   // PUT(update) an auction using Id in the DB
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto auctionDto)
        {
            // getting all the auction 'Items'
            var auction = await _context.Auctions
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);

            // return NotFound() if the DB query was unsuccessful
            if (auction == null) return NotFound();

            // check seller == username
            if (auction.Seller != User.Identity.Name) return Forbid();

            // making changes to the passed Id
            auction.Item.Make = auctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = auctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = auctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = auctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = auctionDto.Year ?? auction.Item.Year;
            
            // publish the changed object
            await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

            // save changes to the DB
            var result = await _context.SaveChangesAsync() > 0;

            // checking the results and returning status
            if (result) return Ok();
            return BadRequest("Problem saving changes");
        }

        //---------------------------------- Endpoint # 4 ----------------------------------
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            // getting the auction by id
            var auction = await _context.Auctions.FindAsync(id);

            // return NotFound() if the DB query was unsuccessful
            if (auction == null) return NotFound();

            // check seller == username
            if (auction.Seller != User.Identity.Name) return Forbid();

            // removing the auction by id
            _context.Auctions.Remove(auction);
            
            // publishing to masstransit
            await _publishEndpoint.Publish<AuctionDeleted>( new {Id = auction.Id.ToString()} );

            // save changes to the DB
            var result = await _context.SaveChangesAsync() > 0;

            // checking the results and returning the status
            if (!result) return BadRequest("Problem saving changes");

            return Ok();
        }

    }
}
