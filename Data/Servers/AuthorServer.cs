using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Servers
{
    public class AuthorServer
    {
        private static readonly Context _context = new();

        public static Task<Author> GetById(int id)
        {
            var author = _context.Authors.AsParallel().FirstOrDefault(x => x.Id == id);
            return Task.FromResult(author);
        }

        public static Task<List<Author>> List
        {
            get
            {
                var list = _context.Authors.AsParallel().ToList();
                return Task.FromResult(list);
            }
        }

        public static Task Create(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Update(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Delete(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
