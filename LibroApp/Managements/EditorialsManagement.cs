using Shared;
using Data.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace LibroApp.Managements
{
    public class EditorialsManagement
    {
        private static readonly string[] menuOptions = new string[]
        {
            "Agregar editorial",
            "Editar editorial",
            "Eliminar editorial",
            "Listar editoriales",
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
            MenuBuilder.Build("Mantenimiento de Editoriales", menuOptions, out int option);
            actions[option]();
        }

        private async static void ListAll()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Lista de Editoriales ", WriteType.WriteLine, 
                Color.Cyan, BackgroundColor.Blue, Style.Bold);
            Log.BreakLine();

            var editorialList = await EditorialServer.List;

            if (!editorialList.Any())
            {
                Log.Print(" No hay editoriales en la base de datos! ", 
                    WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                Log.WaitForKey();
                Init();
            }

            editorialList.AsParallel().OrderByDescending(x => x.Id).ForAll(editorial =>
            {
                Log.Print($"({editorial.Id}) {editorial.Name}", WriteType.WriteLine, 
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
                Log.Print(" Eliminar editoriales ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var editorialList = await EditorialServer.List;

                if (!editorialList.Any())
                {
                    Log.Print(" No hay editoriales en la base de datos! ", WriteType.WriteLine, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                editorialList.AsParallel().OrderByDescending(x => x.Id).ForAll(editorial =>
                {
                    Log.Print($"({editorial.Id}) {editorial.Name}", WriteType.WriteLine,
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

                var editorial = await EditorialServer.GetById(selected);

                if (editorial == null)
                {
                    Log.Print(" No existe una editorial con este código en la lista! ", 
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    continue;
                }

                do
                {
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea eliminar la editorial " +
                        $"{Color.Yellow + Style.Bold}{editorial.Name}{Log.ResetFormat+ Color.Yellow}?", 
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
                        await EditorialServer.Delete(editorial);
                        Log.BreakLine();
                        Log.Print(" Editorial eliminada con exito! ", WriteType.WriteLine, 
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
            string newPhone = "";
            string newCountry = "";
            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Editar editoriales ", WriteType.WriteLine, Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var editorialList = await EditorialServer.List;

                if (!editorialList.Any())
                {
                    Log.Print(" No hay editoriales en la base de datos! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                editorialList.AsParallel().OrderByDescending(x => x.Id).ForAll(editorial =>
                {
                    Log.Print($"({editorial.Id}) {editorial.Name}", WriteType.WriteLine, Color.Cyan, Style.Bold);
                });

                Log.BreakLine();
                Log.Print("Seleccione una opción: ");

                bool successed = int.TryParse(Log.Input(), out int selected);

                if (!successed)
                {
                    Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                    Log.WaitForKey();
                    continue;
                }

                var editorial = await EditorialServer.GetById(selected);

                if (editorial == null)
                {
                    Log.Print(" No existe una editorial con este código en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
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
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                do
                {
                    Log.Print("Digite el nuevo teléfono: ");

                    newPhone = Log.Input();

                    if (string.IsNullOrEmpty(newPhone))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine, Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                do
                {
                    Log.Print("Digite el nuevo país: ");

                    newCountry = Log.Input();

                    if (string.IsNullOrEmpty(newCountry))
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
                    Log.Print($"{Color.Yellow}Seguro desea editar la editorial " +
                        $"{Color.Yellow + Style.Bold}{editorial.Name}{Log.ResetFormat + Color.Yellow} " +
                        $"con estos nuevos valores?", 
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
                        editorial.Name = newName;
                        editorial.Country = newCountry;
                        editorial.Phone = newPhone;
                        await EditorialServer.Update(editorial);
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
                Log.Print(" Agregar nueva editorial ", WriteType.WriteLine, 
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

                string phone;
                do
                {
                    Log.Print("Digite el teléfono: ");

                    phone = Log.Input();

                    if (string.IsNullOrEmpty(phone))
                    {
                        Log.Print(" Debe introducir un valor! ", WriteType.WriteLine, 
                            Color.White, BackgroundColor.Red);
                        Log.WaitForKey();
                        continue;
                    }

                    break;
                } while (true);

                string country;
                do
                {
                    Log.Print("Digite el país: ");

                    country = Log.Input();

                    if (string.IsNullOrEmpty(country))
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
                    Log.Print("Seguro desea agregar esta editorial?", 
                        WriteType.WriteLine, Color.Yellow);
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
                        var editorial = new Editorial
                        {
                            Name = name,
                            Country = country,
                            Phone = phone,
                        };

                        await EditorialServer.Create(editorial);
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
