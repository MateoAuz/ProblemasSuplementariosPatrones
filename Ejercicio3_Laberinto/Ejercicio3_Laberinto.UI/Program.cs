using Ejercicio3_Laberinto.UI.Forms;

namespace Ejercicio3_Laberinto.UI;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
