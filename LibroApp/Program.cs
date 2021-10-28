using Shared;
using LibroApp.Managements;
using System;
using System.Collections.Generic;

namespace LibroApp
{
    public class Program
    {
        private static readonly string[] menuOptions = new string[]
        {
            "Mantenimientos de autores.",
            "Mantenimientos de categorías.",
            "Mantenimientos de editoriales.",
            "Mantenimientos de libro.",
            "Búsqueda de libro",
            "Salir"
        };

        private static readonly Dictionary<int, Action> actions = new()
        {
            { 1, () => AuthorsManagement.Init() },
            { 2, () => CategoriesManagement.Init() },
            { 3, () => EditorialsManagement.Init() },
            { 4, () => BooksManagement.Init() },
            { 5, () => BooksSearch.Init()},
            { 6, () => Environment.Exit(0) },
        };

        public static void Main()
        {
            MenuBuilder.Build("Menu principal", menuOptions, out int option);
            actions[option]();
        }
    }
}
