using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Servers
{
    public class EditorialServer
    {
        private static readonly Context _context = new();

        public static Task<Editorial> GetById(int id)
        {
            var editorial = _context.Editorials.AsParallel().FirstOrDefault(x => x.Id == id);
            return Task.FromResult(editorial);
        }

        public static Task<List<Editorial>> List
        {
            get
            {
                var list = _context.Editorials.AsParallel().ToList();
                return Task.FromResult(list);
            }
        }

        public static Task Create(Editorial editorial)
        {
            _context.Editorials.Add(editorial);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Update(Editorial editorial)
        {
            _context.Editorials.Update(editorial);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Delete(Editorial editorial)
        {
            _context.Editorials.Remove(editorial);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
