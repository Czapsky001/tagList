using Microsoft.AspNetCore.Mvc;
using TagList.Model;
using TagList.Repositories.TagRepo;

namespace TagList.Controllers;
[ApiController]
[Route("[controller]")]
public class TagFromDatabaseController : ControllerBase
{
    private readonly ITagRepository tagRepository;
    private readonly ILogger<TagFromDatabaseController> logger;

    public TagFromDatabaseController(ITagRepository tagRepository, ILogger<TagFromDatabaseController> logger)
    {
        this.tagRepository = tagRepository;
        this.logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tag>>> GetTags([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string sortBy = "name", [FromQuery] string sortOrder = "asc")
    {
        var tags = await tagRepository.GetAllAsync();
        var totalCount = tags.Count();
        var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        if (page >= totalPages)
        {
            logger.LogInformation("There is nothing, enter a smaller number of pages");
            return NotFound();
        }
        if (sortBy == "name")
        {
            tags = sortOrder == "asc" ? tags.OrderBy(tag => tag.Name) : tags.OrderByDescending(tag => tag.Name);
        }
        else if (sortBy == "count")
        {
            tags = sortOrder == "asc" ? tags.OrderBy(tag => tag.Count) : tags.OrderByDescending(tag => tag.Count);
        }
        else
        {
            logger.LogInformation("Invalid sortBy parameter. ");
            tags = tags.OrderBy(tag => tag.Name);
        }
        var tagsOnPage = tags.Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
        return Ok(tagsOnPage);
    }

}
