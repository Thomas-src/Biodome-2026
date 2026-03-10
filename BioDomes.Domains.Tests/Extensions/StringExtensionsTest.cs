using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Tests.Extensions;

[TestFixture]
[TestOf(typeof(StringExtensions))]
public class StringExtensionsTest
{

    [Test]
    public void ToKebabCase_Should_Work()
    {
        string panthera = "Panthera Leo";
        string equus = "Equus quagga";
        string quarcus = "Quercus acerifolia";

        Assert.That(panthera.ToKebabCase(), Is.EqualTo("panthera-leo"));
        Assert.That(equus.ToKebabCase(), Is.EqualTo("equus-quagga"));
        Assert.That(quarcus.ToKebabCase(), Is.EqualTo("quercus-acerifolia"));

    }
}