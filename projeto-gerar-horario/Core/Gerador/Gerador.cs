using Google.OrTools.Sat;

namespace Core;

public class Gerador
{
  public static CpModel PrepararModel(IGerarHorarioOptions gerarHorarioOptions)
  {
    Console.WriteLine(gerarHorarioOptions);

    CpModel model = new CpModel();

    return model;
  }

  public bool GerarHorario(
    IGerarHorarioOptions gerarHorarioOptions,
    bool verbose = true)
  {
    CpModel model = PrepararModel(gerarHorarioOptions);

    // Solve
    CpSolver solver = new CpSolver();
    CpSolverStatus status = solver.Solve(model);

    Console.WriteLine($"Solve status: {status}");

    // Print solution.
    // Check that the problem has a feasible solution.
    if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
    {
      Console.WriteLine($"Total cost: {solver.ObjectiveValue}\n");
    }
    else
    {
      Console.WriteLine("No solution found.");
    }

    if (verbose)
    {
      Console.WriteLine("Statistics");
      Console.WriteLine($"  - conflicts : {solver.NumConflicts()}");
      Console.WriteLine($"  - branches  : {solver.NumBranches()}");
      Console.WriteLine($"  - wall time : {solver.WallTime()}s");
    }


    return true;
  }
}

