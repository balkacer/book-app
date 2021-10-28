using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Servers
{
    public class BookServer
    {
        private static readonly Context _context = new();

        public static Task<List<Book>> GetByName(string name)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Editorial)
                .Include(x => x.Category)
                .AsParallel()
                .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(books);
        }
        public static Task<List<Book>> GetByEditorialCountry(string name)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Editorial)
                .Include(x => x.Category)
                .AsParallel()
                .Where(x => x.Editorial.Country.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(books);
        }

        public static Task<List<Book>> GetByEditorialName(string name)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Editorial)
                .Include(x => x.Category)
                .AsParallel()
                .Where(x => x.Editorial.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(books);
        }

        public static Task<List<Book>> GetByAuthorName(string name)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Editorial)
                .Include(x => x.Category)
                .AsParallel()
                .Where(x => x.Author.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(books);
        }

        public static Task<List<Book>> GetByCategoryName(string name)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Editorial)
                .Include(x => x.Category)
                .AsParallel()
                .Where(x => x.Category.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult(books);
        }

        public static Task<List<Book>> GetByYears(int from, int to)
        {
            var books = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Editorial)
                .Include(x => x.Category)
                .AsParallel()
                .Where(x => int.Parse(x.Year) >= from && int.Parse(x.Year) <= to)
                .ToList();
            return Task.FromResult(books);
        }

        public static Task<Book> GetById(int id)
        {
            var book = _context.Books.AsParallel().FirstOrDefault(x => x.Id == id);
            return Task.FromResult(book);
        }

        public static Task<List<Book>> List
        {
            get
            {
                var list = _context.Books.AsParallel().ToList();
                return Task.FromResult(list);
            }
        }

        public static Task Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public static Task Delete(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
