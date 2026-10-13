using Microsoft.AspNetCore.Mvc;
using CoreLedger.Services;

namespace CoreLedger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagsService _tagsService;

    public TagController(ITagsService tagsService) =>
        _tagsService = tagsService;

    [HttpGet]
    public async Task<List<CoreLedger.Models.Tag>> Get() =>
        await _tagsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<CoreLedger.Models.Tag>> Get(string id)
    {
        var tag = await _tagsService.GetAsync(id);

        if (tag is null)
        {
            return NotFound();
        }

        return tag;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CoreLedger.Models.Tag newTag)
    {
        await _tagsService.CreateAsync(newTag);

        return CreatedAtAction(nameof(Get), new { id = newTag.Id }, newTag);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CoreLedger.Models.Tag updatedTag)
    {
        var tag = await _tagsService.GetAsync(id);

        if (tag is null)
        {
            return NotFound();
        }

        updatedTag.Id = tag.Id;

        await _tagsService.UpdateAsync(id, updatedTag);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var tag = await _tagsService.GetAsync(id);

        if (tag is null)
        {
            return NotFound();
        }

        await _tagsService.RemoveAsync(id);

        return NoContent();
    }
}