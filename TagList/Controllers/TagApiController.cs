using Microsoft.AspNetCore.Mvc;
using System.Net;
using TagList.Convert;
using TagList.Model;
using TagList.Repositories.TagRepo;

namespace TagList.Controllers;

[ApiController]
[Route("[controller]")]
public class TagApiController : ControllerBase
{
    private readonly ILogger<TagApiController> _logger;
    private readonly IConvertJson _converter;
    private readonly ITagRepository _tagRepository;

    public TagApiController(ILogger<TagApiController> logger, IConvertJson converter, ITagRepository tagRepository)
    {
        _logger = logger;
        _converter = converter;
        _tagRepository = tagRepository;
    }

    /// <summary>
    /// Get all tags from the Stack Overflow API.
    /// </summary>
    /// <returns>HTTP response.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllTagsFromApi()
    {
        var handler = new HttpClientHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip;
        using HttpClient client = new HttpClient(handler);
        for (var i = 1; i < 13; i++)
        {

            var apiURL = $"https://api.stackexchange.com/2.3/tags?page={i}&pagesize=100&order=desc&sort=popular&site=stackoverflow";
            var response = await client.GetAsync(apiURL);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    var result = _converter.DeserializeObject(content);
                    if (result != null)
                    {
                        await ProcessTagItems(result);
                    }
                    else
                    {
                        _logger.LogError("Tags are empty");
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    BadRequest("Deserialization error: " + ex.Message);
                }
            }
            else
            {
                _logger.LogError("Request failed. ");
                return BadRequest();
            }
        }
        await CalculateAndSetPercentage();

        return Ok();
    }
    /// <summary>
    /// refresh tags
    /// </summary>
    /// <returns>IActionResult</returns>

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTagsFromApi()
    {
        try
        {
            await GetAllTagsFromApi();
            return Ok("Tags refreshed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error with refreshing tags");
            return BadRequest();
        }
    }

    /// <summary>
    /// calculate "count" in tags
    /// </summary>
    /// <returns></returns>
    private async Task CalculateAndSetPercentage()
    {
        var allTags = await _tagRepository.GetAllAsync();
        var totalCount = allTags.Sum(tag => tag.Count);

        foreach (var tag in allTags)
        {
            tag.Count = Math.Round((tag.Count / totalCount) * 100, 2);
            await _tagRepository.UpdateAsync(tag);
        }
    }




    /// <summary>
    /// add Tags
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private async Task ProcessTagItems(List<Tag> result)
    {
        foreach (var item in result)
        {
            var existTag = await _tagRepository.GetByNameAsync(item.Name);
            if (existTag == null)
            {
                await _tagRepository.AddAsync(item);
            }
        }
    }
}
