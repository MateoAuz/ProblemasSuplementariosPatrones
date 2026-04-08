using Ejercicio02_Caballo.Application.UseCases;
using Ejercicio02_Caballo.Domain.Exceptions;
using Ejercicio02_Caballo.Infrastructure.Services;

namespace Ejercicio02_Caballo.Tests;

[TestClass]
public class SolveKnightTourUseCaseTests
{
    [TestMethod]
    public void Execute_ShouldReturnSuccess_For5x5Board()
    {
        var solver = new KnightTourSolver();
        var useCase = new SolveKnightTourUseCase(solver);

        var result = useCase.Execute(5, 5, 0, 0);

        Assert.IsTrue(result.IsSuccessful);
    }

    [TestMethod]
    public void Execute_ShouldThrowDomainException_WhenPositionIsInvalid()
    {
        var solver = new KnightTourSolver();
        var useCase = new SolveKnightTourUseCase(solver);

        try
        {
            useCase.Execute(5, 5, 10, 10);
            Assert.Fail("Se esperaba una DomainValidationException.");
        }
        catch (DomainValidationException)
        {
            // Exito
        }
    }
}