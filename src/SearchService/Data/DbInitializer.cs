using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        // static call for MongoDb
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
        
        // indexing for search
        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();
        
        // getting the count of items from Mongo DB
        var count = await DB.CountAsync<Item>();

        if (count == 0)
        {
            Console.WriteLine("No data - will attempt to seed");
            
            // getting data related to items
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");
            
            // making options
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
            
            // passing itemData and options to get all data
            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
            
            // saving changes
            await DB.SaveAsync(items);
        }
    }
}