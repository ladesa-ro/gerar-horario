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
        Assert.Pass();
    }
}
