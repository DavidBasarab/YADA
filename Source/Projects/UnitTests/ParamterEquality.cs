using FluentAssertions;
using NUnit.Framework;
using YADA;

namespace UnitTests
{
    [TestFixture]
    [Category("Parameters")]
    public class ParamterEquality
    {
        [Test]
        public void ParameterWithDifferentNamesAndSameValueAreNotEqual()
        {
            var firstParamter = new Parameter("FirstParamter", 17);
            var secondParameter = new Parameter("SecondParamter", 17);

            firstParamter.Should().NotBe(secondParameter);
        }

        [Test]
        public void ParameterWithSameNameAndDifferentValueAreNotEqual()
        {
            var firstParamter = new Parameter("FirstParamter", 17);
            var secondParameter = new Parameter("FirstParamter", "test2");

            firstParamter.Should().NotBe(secondParameter);
        }

        [Test]
        public void ParamtersWithNameAndSqlValueAreEqual()
        {
            var firstParamter = new Parameter("FirstParamter", 17);
            var secondParameter = new Parameter("FirstParamter", 17);

            firstParamter.Should().Be(secondParameter);
        }

        [Test]
        public void ParamtersWithNameAndValueIsNullDoesNotCauseAnErrorAndAreNotEqual()
        {
            var firstParamter = new Parameter("FirstParamter", 17);
            var secondParameter = new Parameter("FirstParamter", null);

            firstParamter.Should().NotBe(secondParameter);
        }
    }
}