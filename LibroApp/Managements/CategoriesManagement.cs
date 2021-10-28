using Shared;
using Data.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace LibroApp.Managements
{
    public class CategoriesManagement
    {
        private static readonly string[] menuOptions = new string[]
        {
            "Agregar categoría",
            "Editar categoría",
            "Eliminar categoría",
            "Listar categorías",
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
            MenuBuilder.Build("Mantenimiento de categorías", menuOptions, out int option);
            actions[option]();
        }

        private async static void ListAll()
        {
            Log.Clear();
            Log.BreakLine();
            Log.Print(" Lista de Categorías ", WriteType.WriteLine, 
                Color.Cyan, BackgroundColor.Blue, Style.Bold);
            Log.BreakLine();

            var categoryList = await CategoryServer.List;

            if (!categoryList.Any())
            {
                Log.Print(" No hay categorías en la base de datos! ", WriteType.WriteLine,
                    Color.Black, BackgroundColor.Yellow);
                Log.WaitForKey();
                Init();
            }

            categoryList.AsParallel().OrderByDescending(x => x.Id).ForAll(category =>
            {
                Log.Print($"({category.Id}) {category.Name}", WriteType.WriteLine, 
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
                Log.Print(" Eliminar Categorías ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var categoryList = await CategoryServer.List;

                if (!categoryList.Any())
                {
                    Log.Print(" No hay categorías en la base de datos! ", 
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                categoryList.AsParallel().OrderByDescending(x => x.Id).ForAll(category =>
                {
                    Log.Print($"({category.Id}) {category.Name}", WriteType.WriteLine, 
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

                var category = await CategoryServer.GetById(selected);

                if (category == null)
                {
                    Log.Print(" No existe un categoría con este código en la lista! ",
                        WriteType.WriteLine, Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    continue;
                }

                do
                {
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea eliminar la categoría " +
                        $"{Color.Yellow + Style.Bold}{category.Name}{Log.ResetFormat+ Color.Yellow}?", 
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
                        await CategoryServer.Delete(category);
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

        private async static void Edit()
        {
            string newName = "";
            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print(" Editar Categorías ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);
                Log.BreakLine();

                var categoryList = await CategoryServer.List;

                if (!categoryList.Any())
                {
                    Log.Print(" No hay categorías en la base de datos! ", WriteType.WriteLine, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    break;
                }

                categoryList.AsParallel().OrderByDescending(x => x.Id).ForAll(category =>
                {
                    Log.Print($"({category.Id}) {category.Name}", WriteType.WriteLine, 
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

                var category = await CategoryServer.GetById(selected);

                if (category == null)
                {
                    Log.Print(" No existe una categoría con este código en la lista! ", WriteType.WriteLine, 
                        Color.Black, BackgroundColor.Yellow);
                    Log.WaitForKey();
                    continue;
                }

                do
                {
                    Log.BreakLine();
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
                    Log.BreakLine();
                    Log.Print($"{Color.Yellow}Seguro desea editar el categoría " +
                        $"{Color.Yellow + Style.Bold}{category.Name}{Log.ResetFormat + Color.Yellow} " +
                        $"con estos nuevos valores?", 
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
                        category.Name = newName;
                        await CategoryServer.Update(category);
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
                Log.Print(" Agregar nueva categoría ", WriteType.WriteLine, 
                    Color.Cyan, BackgroundColor.Blue, Style.Bold);

                string name;
                do
                {
                    Log.BreakLine();
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

                do
                {
                    Log.BreakLine();
                    Log.Print("Seguro desea agregar este categoría?", 
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
                        var category = new Category
                        {
                            Name = name
                        };

                        await CategoryServer.Create(category);
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
