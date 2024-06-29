using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery]SearchParams searchParams)
    {
        // finding all "Item"s from DB using paged search
        var query = DB.PagedSearch<Item, Item>();

        // if the searchTerm is NOT null
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            // do a full text search and sort using the meta-score of each text result
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }
        
        // sorting
        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
            // underscore means default options
        };
        
        // filtering options
        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) 
                                             && x.AuctionEnd < DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        // checking for Seller and Winner
        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }
        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }
        
        // also getting the page number and size number
        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);
        
        // execute query
        var result = await query.ExecuteAsync();
        // return by implementing pagination
        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}