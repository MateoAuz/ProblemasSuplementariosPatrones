namespace TicTacToeInteligente;

internal class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();

        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadKey();
    }
}
