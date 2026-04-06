namespace ProblemaNReinas;

internal class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();

        Console.WriteLine("\nPresiona una tecla para salir...");
        Console.ReadKey();
    }
}
