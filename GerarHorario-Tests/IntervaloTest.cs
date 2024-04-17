using Allure.NUnit;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Tests;

[AllureNUnit]
public class IntervaloTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestVerificarIntervalo()
    {

        /*=======================================
        TESTE
        Intervalo (18:00 - 19:59)
        checar 05:00 -> false
        checar 19:14 -> true
        checar 19:59 -> true
        checar 18:00 -> true
        =======================================*/

        Assert.Multiple(() =>
        {
            var intervalo1 = new Intervalo("07:30", "17:29:59");
            var intervalo2 = new Intervalo("16:40", "17:29:59");

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, intervalo2), Is.True);
        });

        Assert.Multiple(() =>
        {
            var intervalo1 = new Intervalo("18:00", "19:59");

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "05:00"), Is.False);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "17:59"), Is.False);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "17:59:00"), Is.False);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "17:59:59"), Is.False);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "18:00"), Is.True);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "18:00:00"), Is.True);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "18:00:01"), Is.True);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:14"), Is.True);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:58"), Is.True);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:58:00"), Is.True);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:58:59"), Is.True);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:59"), Is.True);
            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:59:00"), Is.True);

            Assert.That(Intervalo.VerificarIntervalo(intervalo1, "19:59:01"), Is.False);


        });
    }

}
