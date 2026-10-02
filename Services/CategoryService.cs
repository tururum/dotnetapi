
using dotnetapi.Models;
using Microsoft.EntityFrameworkCore;
using dotnetapi.Context;


namespace dotnetapi.Services{
public class CategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        return category;
    }

    public async Task<Category> CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> EditCategory(int id, Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            throw new Exception("Category not found");
        }
        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        await _context.SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
}
}
