using Core;
using NUnit.Allure.Core;

namespace Core_Tests;
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

        Assert.IsTrue(metodoTrue);
        Assert.Pass();
    }
}
