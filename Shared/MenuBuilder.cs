namespace Shared
{
    public class MenuBuilder
    {
        public static void Build(string title, string[] options, out int selected)
        {
            do
            {
                Log.Clear();
                Log.BreakLine();
                Log.Print($" {title} \n", WriteType.WriteLine, Color.Black, BackgroundColor.White);

                for (int i = 1; i <= options.Length; i++)
                {
                    var option = options[i - 1];
                    Log.Print($"{i}-{option}", WriteType.WriteLine, Color.Cyan);
                }

                Log.BreakLine();
                Log.Print("Seleccione un opción: ");

                bool successed = int.TryParse(Log.Input(), out selected);

                if (!successed)
                {
                    Log.Print(" Debe introducir un valor numerico! ", WriteType.WriteLine, Color.White, BackgroundColor.Red, Style.Italic);
                    Log.WaitForKey();
                    continue;
                }

                if (selected <= 0 || selected > options.Length)
                {
                    Log.Print(" El valor ingresado no está en la lista! ", WriteType.WriteLine, Color.Black, BackgroundColor.Yellow, Style.Italic);
                    Log.WaitForKey();
                    continue;
                }

                if (successed && selected > 0 && selected <= options.Length)
                    break;
            } while (true);
        }
    }
}
