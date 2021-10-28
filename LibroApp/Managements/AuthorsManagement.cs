using Shared;
using Data.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace LibroApp.Managements
{
    public class AuthorsManagement
    {
        private static readonly string[] menuOptions = new string[]
        {
            "Agregar autor",
            "Editar autor",
            "Eliminar autor",
            "Listar autores",
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
            MenuBuilder.Build("Mantenimiento de Autores", menuOptions, out int option);
            actions[option]();
        }

        private async static void ListAll()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Lista de Autores ", WriteType.WriteLine, 
                Color.Cyan, BackgroundColor.Blue, Style.Bold);
            Log.BreakLine();

            var authorList = await AuthorServer.List;

            if (!authorList.Any())
            {
                Log.Print(" No hay autores en la base de datos! ",
                    WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                Log.WaitForKey();
                Init();
            }

            authorList.AsParallel().OrderByDescending(x => x.Id).ForAll(author =>
            {
                Log.Print($"({author.Id}) {author.Name}", WriteType.WriteLine, 
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
                Log.Print(" Eliminar Autores ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var authorList = await AuthorServer.List;

                if (!authorList.Any())
                {
                    Log.Print(" No hay autores en la base de datos! ", WriteType.WriteLine, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                authorList.AsParallel().OrderByDescending(x => x.Id).ForAll(author =>
                {
                    Log.Print($"({author.Id}) {author.Name}", WriteType.WriteLine, 
                        Color.Cyan, Style.Bold);
                });

                Log.BreakLine();
                Log.Print("Seleccione una opción: ");

                bool successed = int.TryParse(Log.Input(), out int selected);

                if (!successed)
                {
                    Log.Print(" Debe introducir un valor numerico! ", 
                        WriteType.WriteLine, Color.White, BackgroundColor.Red);
                    Log.WaitForKey();
                    continue;
                }

                var author = await AuthorServer.GetById(selected);

                if (author == null)
                {
                    Log.Print(" No existe un autor con este código en la lista! ", 
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    continue;
                }

                do
                {
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea eliminar el autor " +
                        $"{Color.Yellow + Style.Bold}{author.Name}{Log.ResetFormat+ Color.Yellow}?", 
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
                        await AuthorServer.Delete(author);
                        Log.BreakLine();
                        Log.Print(" Autor eliminado con exito! ", WriteType.WriteLine,
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
            string newEmail = "";
            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Editar Autores ", WriteType.WriteLine, Color.Cyan, 
                    BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var authorList = await AuthorServer.List;

                if (!authorList.Any())
                {
                    Log.Print(" No hay autores en la base de datos! ", WriteType.WriteLine, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                authorList.AsParallel().OrderByDescending(x => x.Id).ForAll(author =>
                {
                    Log.Print($"({author.Id}) {author.Name}", WriteType.WriteLine, 
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

                var author = await AuthorServer.GetById(selected);

                if (author == null)
                {
                    Log.Print(" No existe un autor con este código en la lista! ", WriteType.WriteLine, 
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
                    Log.Print("Digite el nuevo correo: ");

                    newEmail = Log.Input();

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
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea editar el autor " +
                        $"{Color.Yellow + Style.Bold}{author.Name}{Log.ResetFormat + Color.Yellow}" +
                        $" con estos nuevos valores?", WriteType.WriteLine);
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
                        author.Email = newEmail;
                        author.Name = newName;
                        await AuthorServer.Update(author);
                        Log.BreakLine();
                        Log.Print(" Cambios guardados con exito! ", WriteType.WriteLine,
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
            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Agregar nuevo autor ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                string name;
                do
                {
                    
                    Log.Print("Digite el nombre: ");

                    name = Log.Input();

                    if (string.IsNullOrEmpty(name))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine, 
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                string email;
                do
                {
                    Log.Print("Digite el correo: ");

                    email = Log.Input();

                    if (string.IsNullOrEmpty(email))
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
                    Log.BreakLine();
                    Log.Print("Seguro desea agregar este autor?", WriteType.WriteLine, Color.Yellow);
                    Log.Print(" Sí [S/s] No [N/n] ", WriteType.Write, Color.Black, BackgroundColor.Yellow);
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
                        var author = new Author
                        {
                            Email = email,
                            Name = name
                        };

                        await AuthorServer.Create(author);
                        Log.BreakLine();
                        Log.Print(" Cambios guardados con exito! ", WriteType.WriteLine,
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
