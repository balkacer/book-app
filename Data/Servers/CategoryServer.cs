using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Servers
{
    public class CategoryServer
    {
        private static readonly Context _context = new();

        public static Task<Category> GetById(int id)
        {
            var category = _context.Categories.AsParallel().FirstOrDefault(x => x.Id == id);
            return Task.FromResult(category);
        }

        public static Task<List<Category>> List
        {
            get
            {
                var list = _context.Categories.AsParallel().ToList();
                return Task.FromResult(list);
            }
        }

        public static Task Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
