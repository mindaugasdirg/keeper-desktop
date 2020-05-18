using Keeper.Desktop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Keeper.Desktop.Services
{
    public class CategoriesService : ModelsService<Category>
    {
        public CategoriesService(DatabaseContext _context) : base(_context) { }

        public override DbSet<Category> GetDbSet(DatabaseContext context) => context.Categories;


        public List<Category> GetActivityOrAllCategories() => context.Categories.Where(c => c.CategoryScope == Category.Scope.Activity || c.CategoryScope == Category.Scope.All).ToList();
        public List<Category> GetTransactionOrAllCategories() => context.Categories.Where(c => c.CategoryScope == Category.Scope.Transaction || c.CategoryScope == Category.Scope.All).ToList();
    }
}
