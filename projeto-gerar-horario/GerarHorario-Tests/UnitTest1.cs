using NUnit.Allure.Core;
using Sisgea.GerarHorario.Core;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

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

        /*=======================================
        TESTE
        Intervalo (18:00 - 19:59)
        checar 05:00 -> false
        checar 19:14 -> true
        checar 19:59 -> true
        checar 18:00 -> true
        =======================================*/
        Intervalo intervalo1 = new Intervalo("18:00", "19:59");
        
        Assert.Multiple(() =>
        {
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "05:00"), Is.False);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:14"), Is.True);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:59"), Is.True);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "18:00"), Is.True);
        });




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

        Assert.That(metodoTrue, Is.True);
        Assert.Pass();
    }
}
