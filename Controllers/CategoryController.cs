

using dotnetapi.Models;
using Microsoft.AspNetCore.Mvc;
using dotnetapi.Services;

namespace dotnetapi.Controllers{
 [ApiController]
  [Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _categoryService.GetCategories();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id)
    {
        return await _categoryService.GetCategoryById(id);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        return await _categoryService.CreateCategory(category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> UpdateCategory([FromRoute] int id, Category category)
    {
        return await _categoryService.EditCategory(id, category);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteCategory(int id)
    {
        return await _categoryService.DeleteCategory(id);
    }
}
}
