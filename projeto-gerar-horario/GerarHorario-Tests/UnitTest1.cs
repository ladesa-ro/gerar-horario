using NUnit.Allure.Core;
using Sisgea.GerarHorario.Core;

namespace Sisgea.GerarHorario.Tests;

[AllureNUnit]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var conexao = new Main();

        bool metodoTrue = conexao.Retorno();
        Assert.That(metodoTrue, Is.True);


        /**
        string intervalo = string.Empty;

        foreach (DiaIntervaloProfessor str in Main.listDiaIntervaloProfessor)
        {
            foreach (DiaIntervaloProfessor otherStr in Main.listDiaIntervaloProfessor)
            {
                if (str.diaDaAula == otherStr.diaDaAula)//SE FOR NO MESMO DIA DE AULA
                {
                    if (str.turma != otherStr.turma)//SE FOR TURMAS DIFERENTES
                    {
                        if (str != otherStr && str.intervaloDaAula == otherStr.intervaloDaAula)//SE FOR A MESMA AULA EM AMBAS TURMAS
                        {
                            //System.Console.WriteLine("Valor repetido encontrado: " + str.intervaloDaAula + "--" + otherStr.intervaloDaAula);
                            if (str.professor == otherStr.professor)
                            {
                                // Assert.Throw();
                                System.Console.WriteLine("O professor " + str.professor + " esta trabalhando no intervalo " + str.intervaloDaAula + " duas vezes nas turmas " + str.turma + " e " + otherStr.turma);
                            }
                        }
                    }
                }

            }
        }
        */

        Assert.Pass();
    }
}
