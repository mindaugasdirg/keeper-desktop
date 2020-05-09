using Keeper.Engine.Models;

namespace Keeper.Engine.Services
{
    public class CategoriesService : ModelsService<Category>
    {
        private DatabaseContext context;

        public CategoriesService(DatabaseContext _context)
        {
            context = _context;
        }

        internal override int Add(Category category)
        {
            if (category is null)
                return -1;
            context.Categories.Add(category);
            return context.SaveChanges();
        }

        internal override int Remove(Category category)
        {
            if (category is null)
                return -1;
            context.Categories.Remove(category);
            return context.SaveChanges();
        }

        internal override int Update(Category updated)
        {
            if (updated is null)
                return -1;
            context.Categories.Update(updated);
            return context.SaveChanges();
        }
    }
}
