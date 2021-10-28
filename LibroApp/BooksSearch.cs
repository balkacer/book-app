using Data.Models;
using Data.Servers;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibroApp
{
    public class BooksSearch
    {
        private static readonly string[] menuOptions = new string[]
        {
            "Búsqueda por nombre del libro.",
            "Búsqueda por el país del editorial.",
            "Búsqueda por año de publicación del libro.",
            "Búsqueda por nombre del autor.",
            "Búsqueda por nombre de la editorial.",
            "Búsqueda por categoría del libro.",
            "Volver atrás"
        };

        private static readonly Dictionary<int, Action> actions = new()
        {
            { 1, () => ByName() },
            { 2, () => ByEditorialCountry() },
            { 3, () => ByYear() },
            { 4, () => ByAuthorName() },
            { 5, () => ByEditorialName() },
            { 6, () => ByCategory() },
            { 7, () => Program.Main() }
        };
        public static void Init()
        {
            MenuBuilder.Build("Búsqueda de libros", menuOptions, out int option);
            actions[option]();
        }

        private static void OrderBy(List<Book> books)
        {
            string[] options = new string[]
            {
                "Nombre del libro (Alfabéticamente).",
                "País del editorial (Alfabéticamente).",
                "Año de publicación del libro (desde el más reciente al más antiguo).",
                "Nombre del autor (Alfabéticamente).",
                "Nombre de la editorial (Alfabéticamente).",
                "Categoría del libro (Alfabéticamente)."
            };

            Dictionary<int, Func<OrderedParallelQuery<Book>>> orders = new()
            {
                { 1, () => books.AsParallel().OrderBy(x => x.Name) },
                { 2, () => books.AsParallel().OrderBy(x => x.Editorial.Country) },
                { 3, () => books.AsParallel().OrderByDescending(x => x.Year) },
                { 4, () => books.AsParallel().OrderBy(x => x.Author.Name) },
                { 5, () => books.AsParallel().OrderBy(x => x.Editorial.Name) },
                { 6, () => books.AsParallel().OrderBy(x => x.Category.Name) }
            };

            MenuBuilder.Build("Ordenar por:", options, out int option);

            orders[option]().ToList().ForEach(book =>
            {
                Log.BreakLine();
                Log.Print("Nombre del libro -------->", WriteType.Write,
                    Color.Yellow, Style.Bold);
                Log.Print(book.Name, WriteType.WriteLine,
                    Color.Yellow);
                Log.Print("Año de publicación ------>", WriteType.Write,
                    Color.Yellow, Style.Bold);
                Log.Print(book.Year, WriteType.WriteLine,
                    Color.Yellow);
                Log.Print("País de origen ---------->", WriteType.Write,
                    Color.Yellow, Style.Bold);
                Log.Print(book.Editorial.Country, WriteType.WriteLine,
                    Color.Yellow);
                Log.Print("Nombre de la editorial -->", WriteType.Write,
                    Color.Yellow, Style.Bold);
                Log.Print(book.Editorial.Name, WriteType.WriteLine,
                    Color.Yellow);
                Log.Print("Nombre del autor -------->", WriteType.Write,
                    Color.Yellow, Style.Bold);
                Log.Print(book.Author.Name, WriteType.WriteLine,
                    Color.Yellow);
                Log.Print("Categoría del libro ----->", WriteType.Write,
                    Color.Yellow, Style.Bold);
                Log.Print(book.Category.Name, WriteType.WriteLine,
                    Color.Yellow);
            });

            Log.BreakLine();
            Log.Print(" Fin de la lista ", WriteType.WriteLine, BackgroundColor.White, Color.Red);
            Log.BreakLine();
            Log.Print("Preciones cualquier tecla para volver al menú...");
            Log.WaitForKey();
            Init();
        }

        private static void ByCategory()
        {
            var books = new List<Book>();
            OrderBy(books);
        }

        private async static void ByEditorialName()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Busqueda por nombre de la editorial ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue);
            Log.BreakLine();
            Log.Print("Digite el nombre o parte del nombre de la editorial: ");
            var books = await BookServer.GetByEditorialName(Log.Input());

            if (!books.Any())
            {
                Log.BreakLine();
                Log.Print(" Ningun libro coincide con la busqueda! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                Log.WaitForKey();
                Init();
            }

            Log.BreakLine();
            Log.Print($"({books.Count}) libro/s encontrado/s", WriteType.WriteLine,
                    Color.Magenta);
            Log.BreakLine();
            Log.Print("Presione una tecla para continuar...");
            Log.WaitForKey();
            OrderBy(books);
        }

        private async static void ByAuthorName()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Busqueda por nombre del autor ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue);
            Log.BreakLine();
            Log.Print("Digite el nombre o parte del nombre del autor: ");
            var books = await BookServer.GetByAuthorName(Log.Input());

            if (!books.Any())
            {
                Log.BreakLine();
                Log.Print(" Ningun libro coincide con la busqueda! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                Log.WaitForKey();
                Init();
            }

            Log.BreakLine();
            Log.Print($"({books.Count}) libro/s encontrado/s", WriteType.WriteLine,
                    Color.Magenta);
            Log.BreakLine();
            Log.Print("Presione una tecla para continuar...");
            Log.WaitForKey();
            OrderBy(books);
        }

        private async static void ByYear()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Busqueda por año de publicación ( desde - hasta ) ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue);
            Log.BreakLine();

            int from;
            do
            {
                Log.Print("Digite el año desde donde desea comenzar a buscar: ");
                bool successed = int.TryParse(Log.Input(), out from);

                if (!successed)
                {
                    Log.BreakLine();
                    Log.Print(" Debe introducir un valor numérico! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                    Log.WaitForKey();
                    continue;
                }

                break;
            } while (true);

            int to;
            do
            {
                Log.Print("Digite el año hasta donde desea buscar: ");
                bool successed = int.TryParse(Log.Input(), out to);

                if (!successed)
                {
                    Log.BreakLine();
                    Log.Print(" Debe introducir un valor numérico! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                    Log.WaitForKey();
                    continue;
                }

                break;
            } while (true);

            var books = await BookServer.GetByYears(from, to);

            if (!books.Any())
            {
                Log.BreakLine();
                Log.Print(" Ningun libro coincide con la busqueda! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                Log.WaitForKey();
                Init();
            }

            Log.BreakLine();
            Log.Print($"({books.Count}) libro/s encontrado/s", WriteType.WriteLine,
                    Color.Magenta);
            Log.BreakLine();
            Log.Print("Presione una tecla para continuar...");
            Log.WaitForKey();
            OrderBy(books);
        }

        private async static void ByEditorialCountry()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Busqueda por país de editorial ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue);
            Log.BreakLine();
            Log.Print("Digite el nombre o parte del nombre del país: ");
            var books = await BookServer.GetByEditorialCountry(Log.Input());

            if (!books.Any())
            {
                Log.BreakLine();
                Log.Print(" Ningun libro coincide con la busqueda! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                Log.WaitForKey();
                Init();
            }

            Log.BreakLine();
            Log.Print($"({books.Count}) libro/s encontrado/s", WriteType.WriteLine,
                    Color.Magenta);
            Log.BreakLine();
            Log.Print("Presione una tecla para continuar...");
            Log.WaitForKey();
            OrderBy(books);
        }

        private async static void ByName()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Busqueda por nombre de libro ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue);
            Log.BreakLine();
            Log.Print("Digite el nombre o parte del nombre del libro: ");
            var books = await BookServer.GetByName(Log.Input());

            if (!books.Any())
            {
                Log.BreakLine();
                Log.Print(" Ningun libro coincide con la busqueda! ", WriteType.WriteLine, Color.Black, BackgroundColor.Red);
                Log.WaitForKey();
                Init();
            }

            Log.BreakLine();
            Log.Print($"({books.Count}) libro/s encontrado/s", WriteType.WriteLine,
                    Color.Magenta);
            Log.BreakLine();
            Log.Print("Presione una tecla para continuar...");
            Log.WaitForKey();
            OrderBy(books);
        }
    }
}
