using Ejercicio02_Caballo.Application.UseCases;
using Ejercicio02_Caballo.Domain.Interfaces;
using Ejercicio02_Caballo.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ejercicio02_Caballo.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        var mainForm = serviceProvider.GetRequiredService<Form1>();

        System.Windows.Forms.Application.Run(mainForm);
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddTransient<IKnightTourSolver, KnightTourSolver>();
        services.AddTransient<SolveKnightTourUseCase>();
        services.AddTransient<Form1>();
    }
}