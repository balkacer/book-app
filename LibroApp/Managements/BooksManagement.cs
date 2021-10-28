using Shared;
using Data.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace LibroApp.Managements
{
    public class BooksManagement
    {
        private static readonly string[] menuOptions = new string[]
        {
            "Agregar libro",
            "Editar libro",
            "Eliminar libro",
            "Listar libros",
            "Volver atrás"
        };

        private static readonly Dictionary<int, Action> actions = new()
        {
            { 1, () => Add() },
            { 2, () => Edit() },
            { 3, () => Delete() },
            { 4, () => ListAll() },
            { 5, () => Program.Main() }
        };

        public static void Init()
        {
            MenuBuilder.Build("Mantenimiento de libros", menuOptions, out int option);
            actions[option]();
        }

        private async static void ListAll()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Lista de Libros ", WriteType.WriteLine, 
                Color.Cyan, BackgroundColor.Blue, Style.Bold);
            Log.BreakLine();

            var bookList = await BookServer.List;

            if (!bookList.Any())
            {
                Log.Print(" No hay libros en la base de datos! ", WriteType.WriteLine, 
                    Color.Black, BackgroundColor.Yellow);
                Log.WaitForKey();
                Init();
            }

            bookList.AsParallel().OrderBy(x => x.Name).ForAll(book =>
            {
                Log.Print($"({book.Id}) {book.Name}", WriteType.WriteLine, 
                    Color.Cyan, Style.Bold);
            });

            Log.BreakLine();
            Log.Print("Precione cualquier tecla para volver al menú...",
                WriteType.Write, Style.Faint);
            Log.WaitForKey();
            Init();
        }

        private async static void Delete()
        {
            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Eliminar Libros ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var bookList = await BookServer.List;

                if (!bookList.Any())
                {
                    Log.Print(" No hay libros en la base de datos! ", WriteType.WriteLine, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                bookList.AsParallel().OrderByDescending(x => x.Id).ForAll(book =>
                {
                    Log.Print($"({book.Id}) {book.Name}", WriteType.WriteLine, 
                        Color.Cyan, Style.Bold);
                });

                Log.BreakLine();
                Log.Print("Seleccione una opción: ");

                bool successed = int.TryParse(Log.Input(), out int selected);

                if (!successed)
                {
                    Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine,
                        Color.White, BackgroundColor.Red);
                    Log.WaitForKey();
                    continue;
                }

                var book = await BookServer.GetById(selected);

                if (book == null)
                {
                    Log.Print(" No existe un libro con este código en la lista! ", 
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    continue;
                }

                do
                {
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea eliminar el libro " +
                        $"{Color.Yellow + Style.Bold}{book.Name}{Log.ResetFormat + Color.Yellow}?", 
                        WriteType.WriteLine);
                    Log.Print("Sí [S/s] No [N/n]", WriteType.Write, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.Print("");

                    var wantContinue = Log.Input().ToLower();

                    if (wantContinue != "s" && wantContinue != "n")
                    {
                        Log.BreakLine();
                        Log.Print(" Debe introducir S, s, N o n! ", WriteType.WriteLine,
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    if (wantContinue == "s")
                    {
                        await BookServer.Delete(book);
                        Log.BreakLine();
                        Log.Print(" Libro eliminado con exito! ", WriteType.WriteLine,
                            Color.Black, BackgroundColor.Green);
                        Log.BreakLine();
                        Log.Print("Precione cualquier tecla para volver al menú...",
                            WriteType.Write, Style.Faint);
                        Log.WaitForKey();
                    }
                    break;
                } while (true);
                break;
            } while (true);
            Init();
        }

        private async static void Edit()
        {
            string newName = "";
            string newYear = "";
            int newCategoryId = 0;
            int newEditorialId = 0;
            int newAuthorId = 0;

            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Editar Libros ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var categoryList = await CategoryServer.List;
                var authorList = await AuthorServer.List;
                var editorialList = await EditorialServer.List;

                var anyCategory = categoryList.Any();
                var anyAuthor = authorList.Any();
                var anyEditorial = editorialList.Any();

                if (!anyCategory || !anyAuthor || !anyEditorial)
                {
                    Log.Print(" No hay categorías, autores o editoriales en la base de datos. ",
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.Print(" No se pueden crear libros sin estos datos! ",
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    Init();
                }

                var bookList = await BookServer.List;

                if (!bookList.Any())
                {
                    Log.Print(" No hay libros en la base de datos! ", 
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                bookList.AsParallel().OrderByDescending(x => x.Id).ForAll(book =>
                {
                    Log.Print($"({book.Id}) {book.Name}", WriteType.WriteLine, 
                        Color.Cyan, Style.Bold);
                });

                Log.BreakLine();
                Log.Print("Seleccione una opción: ");

                bool successed = int.TryParse(Log.Input(), out int selected);

                if (!successed)
                {
                    Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, 
                        Color.White, BackgroundColor.Red);
                    Log.WaitForKey();
                    continue;
                }

                var book = await BookServer.GetById(selected);

                if (book == null)
                {
                    Log.Print(" No existe un libro con este código en la lista! ", WriteType.WriteLine,
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    continue;
                }

                Log.BreakLine();

                do
                {
                    Log.Print("Digite el nuevo nombre: ");

                    newName = Log.Input();

                    if (string.IsNullOrEmpty(newName))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine, 
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                do
                {
                    Log.Print("Digite el nuevo año: ");

                    newYear = Log.Input();

                    if (string.IsNullOrEmpty(newYear))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine,
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                Log.BreakLine();
                Log.Print(" Lista de Categorías ", WriteType.WriteLine, Color.White, BackgroundColor.Magenta);
                Log.BreakLine();

                categoryList.AsParallel().OrderByDescending(x => x.Id).ForAll(category =>
                {
                    Log.Print($"({category.Id}) {category.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                do
                {
                    Log.BreakLine();
                    Log.Print("Seleccione la categoría: ");

                    bool catSuccessed = int.TryParse(Log.Input(), out int catSelected);

                    if (!catSuccessed)
                    {
                        Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    var category = categoryList.AsParallel().FirstOrDefault(x => x.Id == catSelected);

                    if (category == null)
                    {
                        Log.Print(" No existe un libro con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                        Log.WaitForKey();
                        continue;
                    }

                    newCategoryId = category.Id;

                    break;
                } while (true);

                Log.BreakLine();
                Log.Print(" Lista de Editoriales ", WriteType.WriteLine, Color.White, BackgroundColor.Magenta);
                Log.BreakLine();

                editorialList.AsParallel().OrderByDescending(x => x.Id).ForAll(editorial =>
                {
                    Log.Print($"({editorial.Id}) {editorial.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                do
                {
                    Log.BreakLine();
                    Log.Print("Seleccione la editorial: ");

                    bool edtSuccessed = int.TryParse(Log.Input(), out int edtSelected);

                    if (!edtSuccessed)
                    {
                        Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    var editorial = editorialList.AsParallel().FirstOrDefault(x => x.Id == edtSelected);

                    if (editorial == null)
                    {
                        Log.Print(" No existe un libro con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                        Log.WaitForKey();
                        continue;
                    }

                    newEditorialId = editorial.Id;

                    break;
                } while (true);

                Log.BreakLine();
                Log.Print(" Lista de Autores ", WriteType.WriteLine, Color.White, BackgroundColor.Magenta);
                Log.BreakLine();

                authorList.AsParallel().OrderByDescending(x => x.Id).ForAll(author =>
                {
                    Log.Print($"({author.Id}) {author.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                do
                {
                    Log.BreakLine();
                    Log.Print("Seleccione el autor: ");

                    bool autSuccessed = int.TryParse(Log.Input(), out int autSelected);

                    if (!autSuccessed)
                    {
                        Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    var author = authorList.AsParallel().FirstOrDefault(x => x.Id == autSelected);

                    if (author == null)
                    {
                        Log.Print(" No existe un autor con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                        Log.WaitForKey();
                        continue;
                    }

                    newAuthorId = author.Id;

                    break;
                } while (true);

                do
                {
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea editar el libro " +
                        $"{Color.Yellow + Style.Bold}{book.Name}{Log.ResetFormat + Color.Yellow}" +
                        $"\n con estos nuevos valores?", 
                        WriteType.WriteLine);
                    Log.Print("Sí [S/s] No [N/n]", WriteType.Write, Color.Black, BackgroundColor.Yellow);
                    Log.Print("");

                    var wantContinue = Log.Input().ToLower();

                    if (wantContinue != "s" && wantContinue != "n")
                    {
                        Log.BreakLine();
                        Log.Print(" Debe introducir S, s, N o n! ", WriteType.WriteLine,
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    if (wantContinue == "s")
                    {
                        book.Year = newYear;
                        book.Name = newName;
                        book.EditorialId = newEditorialId;
                        book.CategoryId = newCategoryId;
                        book.AuthorId = newAuthorId;

                        await BookServer.Update(book);
                        Log.BreakLine();
                        Log.Print(" Caambios guardados con exito! ", WriteType.WriteLine,
                            Color.Black, BackgroundColor.Green);
                        Log.BreakLine();
                        Log.Print("Precione cualquier tecla para volver al menú...",
                            WriteType.Write, Style.Faint);
                        Log.WaitForKey();
                    }
                    break;
                } while (true);

                break;
            } while (true);
            Init();
        }

        private async static void Add()
        {
            string newName = "";
            string newYear = "";
            int newCategoryId = 0;
            int newEditorialId = 0;
            int newAuthorId = 0;

            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Agregar nuevo libro ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                do
                {
                    Log.Print("Digite el nombre: ");

                    newName = Log.Input();

                    if (string.IsNullOrEmpty(newName))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine,
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                do
                {
                    Log.Print("Digite el año: ");

                    newYear = Log.Input();

                    if (string.IsNullOrEmpty(newYear))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine,
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                Log.BreakLine();
                Log.Print(" Lista de Categorías ", WriteType.WriteLine, Color.White, BackgroundColor.Magenta);
                Log.BreakLine();

                var categoryList = await CategoryServer.List;

                categoryList.AsParallel().OrderByDescending(x => x.Id).ForAll(category =>
                {
                    Log.Print($"({category.Id}) {category.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                do
                {
                    Log.BreakLine();
                    Log.Print("Seleccione la categoría: ");

                    bool catSuccessed = int.TryParse(Log.Input(), out int catSelected);

                    if (!catSuccessed)
                    {
                        Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    var category = categoryList.AsParallel().FirstOrDefault(x => x.Id == catSelected);

                    if (category == null)
                    {
                        Log.Print(" No existe un libro con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                        Log.WaitForKey();
                        continue;
                    }

                    newCategoryId = category.Id;

                    break;
                } while (true);

                Log.BreakLine();
                Log.Print(" Lista de Editoriales ", WriteType.WriteLine, Color.White, BackgroundColor.Magenta);
                Log.BreakLine();

                var editorialList = await EditorialServer.List;

                editorialList.AsParallel().OrderByDescending(x => x.Id).ForAll(editorial =>
                {
                    Log.Print($"({editorial.Id}) {editorial.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                do
                {
                    Log.BreakLine();
                    Log.Print("Seleccione la editorial: ");

                    bool edtSuccessed = int.TryParse(Log.Input(), out int edtSelected);

                    if (!edtSuccessed)
                    {
                        Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    var editorial = editorialList.AsParallel().FirstOrDefault(x => x.Id == edtSelected);

                    if (editorial == null)
                    {
                        Log.Print(" No existe un libro con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                        Log.WaitForKey();
                        continue;
                    }

                    newEditorialId = editorial.Id;

                    break;
                } while (true);

                Log.BreakLine();
                Log.Print(" Lista de Autores ", WriteType.WriteLine, Color.White, BackgroundColor.Magenta);
                Log.BreakLine();

                var authorList = await AuthorServer.List;

                authorList.AsParallel().OrderByDescending(x => x.Id).ForAll(author =>
                {
                    Log.Print($"({author.Id}) {author.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                do
                {
                    Log.BreakLine();
                    Log.Print("Seleccione el autor: ");

                    bool autSuccessed = int.TryParse(Log.Input(), out int autSelected);

                    if (!autSuccessed)
                    {
                        Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    var author = authorList.AsParallel().FirstOrDefault(x => x.Id == autSelected);

                    if (author == null)
                    {
                        Log.Print(" No existe un autor con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                        Log.WaitForKey();
                        continue;
                    }

                    newAuthorId = author.Id;

                    break;
                } while (true);

                do
                {
                    Log.BreakLine();
                    Log.Print("Seguro desea agregar este libro?", WriteType.WriteLine, Color.Yellow);
                    Log.Print(" Sí [S/s] No [N/n] ", WriteType.Write,
                        Color.Black, BackgroundColor.Yellow);
                    Log.Print("");

                    var wantContinue = Log.Input().ToLower();

                    if (wantContinue != "s" && wantContinue != "n")
                    {
                        Log.BreakLine();
                        Log.Print(" Debe introducir S, s, N o n! ", WriteType.WriteLine, 
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    if (wantContinue == "s")
                    {
                        var book = new Book
                        {
                            Year = newYear,
                            Name = newName,
                            EditorialId = newEditorialId,
                            CategoryId = newCategoryId,
                            AuthorId = newAuthorId
                        };

                        await BookServer.Create(book);
                        Log.BreakLine();
                        Log.Print(" Caambios guardados con exito! ", WriteType.WriteLine,
                            Color.Black, BackgroundColor.Green);
                        Log.BreakLine();
                        Log.Print("Precione cualquier tecla para volver al menú...",
                            WriteType.Write, Style.Faint);
                        Log.WaitForKey();
                    }
                    break;
                } while (true);
                break;
            } while (true);
            Init();
        }
    }
}
