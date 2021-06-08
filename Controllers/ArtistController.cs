using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using firstWebAPI.Clients;
using firstWebAPI.Extentions;
using firstWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace firstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly MusicClient _musicClient;
        private readonly IDynamoDbClient _dynamoDbClient;
        public ArtistController(ILogger<ArtistController> logger, MusicClient musicClient, IDynamoDbClient dynamoDbClient)
        {
            _logger = logger;
            _musicClient = musicClient;
            _dynamoDbClient = dynamoDbClient;
        }


        [HttpGet("inf")]
        public async Task<ArtistResponse> GetInfo([FromQuery] ArtistParameters parameters)
        {

            var inf = await _musicClient.GetInfo(parameters.NameOfArtist);
            var result = new ArtistResponse
            {
                Artist = inf.Artist.Name,
                Description = inf.Artist.Bio.Summary,
            };
            return result;
        }

        [HttpGet("albums")]
        public async Task<ArtistResponse2> GetTopAlbums([FromQuery] ArtistParameters parameters)
        {

            var inf = await _musicClient.GetTopAlbums(parameters.NameOfArtist);
            var result = new ArtistResponse2
            {
                Albums = inf.Topalbums.Album.Take(3).ToList(),

            };
            return result;
        }

        [HttpGet("tracks")]
        public async Task<ArtistResponse3> GetTopTracks([FromQuery] ArtistParameters parameters)
        {

            var inf = await _musicClient.GetTopTracks(parameters.NameOfArtist);
            var result = new ArtistResponse3
            {
                Tracks = inf.Toptracks.Track.Take(3).ToList(),
            };
            return result;
        }

        [HttpGet("similar")]
        public async Task<ArtistResponse4> GetSimilar([FromQuery] ArtistParameters parameters)
        {

            var inf = await _musicClient.GetSimilar(parameters.NameOfArtist);
            var result = new ArtistResponse4
            {
                Similar = inf.Similarartists.Artist.Take(3).ToList(),
            };
            return result;
        }
        

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFavorite([FromQuery] string artist)
        {
            var result = await _dynamoDbClient.GetDataByName(artist);
            if (result==null)
                return NotFound("Record doesn't exist in database");
            var artistResponse = new ArtistResponse
            {
                Artist = result.Artist,
                Description = result.Description
            };
            return Ok(artistResponse);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavorites([FromBody] ArtistResponse artist)
        {
            var data = new ArtistDbRepository
            {
                Artist = artist.Artist,
                Description = artist.Description
            };

            await _dynamoDbClient.PostDataToDb(data);
            return Ok("Ok");
        }


        [HttpGet("all_favorites")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _dynamoDbClient.GetAll();
            if (response == null)
                return NotFound("There are no records in db");
            var result = response
                .Select(x => new ArtistResponse()
                {
                    Artist = x.Artist,
                    Description = x.Description,
                })
                .ToList();
            return Ok(result);

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string artist)
        {
            var result = await _dynamoDbClient.GetDataByName(artist);
            if (result == null)
                return NotFound("Record doesn't exist in database");
            await _dynamoDbClient.Delete(artist);
          
            return Ok();
        }

    }
}
