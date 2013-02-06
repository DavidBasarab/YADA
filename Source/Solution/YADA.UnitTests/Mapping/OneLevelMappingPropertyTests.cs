using FluentAssertions;
using NUnit.Framework;
using Yada;

namespace YADA.UnitTests.Mapping
{
    [TestFixture]
    [Category("YADA.UnitTests.Mapping")]
    public class OneLevelMappingPropertyTests : MappingTest
    {
        private const int PERSON_ID_VALUE = 4891;
        private const string LAST_NAME_VALUE = "Seaborn";
        private const string FIRST_NAME_VALUE = "Sam";
        private const string HOUSE_VALUE = "House of Representatives";
        private const int COMPOSITE_ID_VALUE = 965879;

        [Test]
        public void WillMapAOneLevelPropertyObject()
        {
            CreateReader();

            reader.Setup(v => v.Read()).Returns(true);

            reader.Setup(v => v.FieldCount).Returns(5);

            reader.Setup(v => v.GetName(0)).Returns("PersonId");
            reader.Setup(v => v.GetName(1)).Returns("CompositeId");
            reader.Setup(v => v.GetName(2)).Returns("LAST_NAME");
            reader.Setup(v => v.GetName(3)).Returns("FirstName");
            reader.Setup(v => v.GetName(4)).Returns("HouseName");

            reader.Setup(v => v.GetValue(0)).Returns(PERSON_ID_VALUE);
            reader.Setup(v => v.GetValue(1)).Returns(COMPOSITE_ID_VALUE);
            reader.Setup(v => v.GetValue(2)).Returns(LAST_NAME_VALUE);
            reader.Setup(v => v.GetValue(3)).Returns(FIRST_NAME_VALUE);
            reader.Setup(v => v.GetValue(4)).Returns(HOUSE_VALUE);

            var result = Creator<OneLevelObject>.Create(reader.Object);

            result.Id.Should().Be(COMPOSITE_ID_VALUE);
            result.Name.Should().Be(HOUSE_VALUE);
            result.SimpleValueObject.Should().NotBeNull();
            result.SimpleValueObject.Id.Should().Be(PERSON_ID_VALUE);
            result.SimpleValueObject.FirstName.Should().Be(FIRST_NAME_VALUE);
            result.SimpleValueObject.LastName.Should().Be(LAST_NAME_VALUE);
        }
    }

}