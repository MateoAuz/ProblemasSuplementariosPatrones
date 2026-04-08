using Ejercicio02_Caballo.Application.UseCases;
using Ejercicio02_Caballo.Domain.Exceptions;
using Ejercicio02_Caballo.Domain.ValueObjects;
using Ejercicio02_Caballo.Infrastructure.Services;

namespace Ejercicio02_Caballo.UI;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Problema del Caballo de Ajedrez (Knight's Tour) ===");
        
        try
        {
            int rows = ReadInteger("Ingresa el número de filas del tablero: ");
            int columns = ReadInteger("Ingresa el número de columnas del tablero: ");
            
            Console.WriteLine($"\nEl tablero es de {columns}x{rows}. Usa índices desde 0 hasta {columns - 1} para X, y 0 hasta {rows - 1} para Y.");
            int startX = ReadInteger("Ingresa la posición inicial en X (columna): ");
            int startY = ReadInteger("Ingresa la posición inicial en Y (fila): ");

            var solver = new KnightTourSolver();
            var useCase = new SolveKnightTourUseCase(solver);

            Console.WriteLine("\nCalculando el recorrido, por favor espera...");
            var result = useCase.Execute(rows, columns, startX, startY);

            if (result.IsSuccessful)
            {
                Console.WriteLine("\n¡Recorrido completado con éxito!");
                PrintBoard(rows, columns, result.Path);
            }
            else
            {
                Console.WriteLine("\nNo se ha encontrado una solución para este tablero y posición inicial.");
            }
        }
        catch (DomainValidationException ex)
        {
            Console.WriteLine($"\n[Error de Validación] {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[Error Inesperado] Ocurrió un error: {ex.Message}");
        }
        
        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    private static int ReadInteger(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            
            if (int.TryParse(input, out int value))
            {
                return value;
            }
            
            Console.WriteLine("Entrada inválida. Por favor, ingresa un número entero.");
        }
    }

    private static void PrintBoard(int rows, int columns, System.Collections.Generic.IReadOnlyList<Position> path)
    {
        int[,] board = new int[columns, rows];
        
        // Initialize board
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = -1;
            }
        }

        // Fill board with jump sequence numbers
        for (int i = 0; i < path.Count; i++)
        {
            var pos = path[i];
            board[pos.X, pos.Y] = i + 1; // 1-indexed moves for display
        }

        // Print board nicely
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int moveNumber = board[x, y];
                if (moveNumber != -1)
                {
                    Console.Write($"{moveNumber,4} ");
                }
                else
                {
                    Console.Write("   - ");
                }
            }
            Console.WriteLine();
        }
    }
}