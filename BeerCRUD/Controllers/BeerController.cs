using BeerCRUD.Data;
using BeerCRUD.Entities;
using BeerCRUD.EntityDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BeerCRUD.Controllers;

[ApiController]
[Route("_api/[controller]/[action]")]
public class BeerController : ControllerBase
{
    private readonly ILogger<BeerController> _logger;
    private readonly ApplicationDB _dbContext;
    public BeerController(ILogger<BeerController> logger,ApplicationDB dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IEnumerable<BeerResponseDTO>> GetBeerList()
    {
       var beerList= _dbContext.Beers.Select(i=>new BeerResponseDTO()
       {
        Name   = i.Name,
        Rating = i.AvgRating,
        Type = i.Type
       });
       return beerList;
    }

    [HttpGet]
    public async Task<IEnumerable<BeerResponseDTO>> SearchBeers([FromQuery]string input)
    {
        var beerList= _dbContext.Beers.Where(i=>i.Name.ToLower().Contains(input.ToLower())).Select(i=>new BeerResponseDTO()
        {
            Name   = i.Name,
            Rating = i.AvgRating,
            Type = i.Type
        });
        return beerList;
    }
    [HttpPost]
    public async Task<IActionResult> AddBeerToList([FromBody] BeerRequestDTO beerRequestDto)
    {
        Beer beer = new Beer()
        {
            Name = beerRequestDto.Name,
            Type = beerRequestDto.Type
        };   
        _dbContext.Add(beer);
       await _dbContext.SaveChangesAsync();
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> RateBeer([FromQuery] int id,[FromQuery] int rating=10)
    {
        var validRating = new List<int>() { 1, 2, 3, 4, 5};
        if (!validRating.Contains(rating))
        {
            return BadRequest("Rating isn't valid. 1 to 5 is allowed");
        }
        if(id!=0)
        {
            try
            {
                var beer = _dbContext.Beers.FirstOrDefault(i => i.BeerId == id);
                beer.TempRating=rating;
                _dbContext.Update(beer);
                await _dbContext.SaveChangesAsync();
                return Ok(beer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        return BadRequest("Please provide an id.");

    }
    [HttpDelete]
    public async Task<IActionResult> DeleteBeer([FromQuery] int id)
    {
            try
            {
                var beer = _dbContext.Beers.FirstOrDefault(i => i.BeerId == id);
                _dbContext.Remove(beer);
                await _dbContext.SaveChangesAsync();
                return Ok($"Removed beer: {beer.Name}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
    }
}