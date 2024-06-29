using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm)
    {
        // finding all "Item"s from DB
        var query = DB.Find<Item>();
        // sorting using Make property in Ascending order
        query.Sort(x => x.Ascending(a => a.Make));

        // if the searchTerm is NOT null
        if (!string.IsNullOrEmpty(searchTerm))
        {
            // do a full text search and sort using the meta-score of each text result
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }
        
        // execute query
        var result = await query.ExecuteAsync();
        // return the list of items
        return result;
    }
}