using FluentAssertions;
using NUnit.Framework;
using Yada;

namespace YADA.UnitTests.Mapping.Value
{
    [TestFixture]
    [Category("YADA.UnitTests.Mapping")]
    public class CreateSimpleObjectFromReaderBasedOnMapping : MappingTest
    {
        private const string FIRST_NAME = "FIRST_NAME";
        private const string LAST_NAME = "LAST_NAME";
        private const string ID_NUMBER = "ID_NUMBER";
        private const int FIRST_NAME_ORDINAL_VALUE = 0;
        private const int LAST_NAME_ORDINAL = 1;
        private const int ID_NUMBER_ORDINAL = 2;
        private const int FIELD_COUNT = 3;
        private const string FIRST_NAME_VALUE = "Luke";
        private const string LAST_NAME_VALUE = "Skywalker";
        private const int ID_VALUE = 18979;

        private void SetUpFirstRead()
        {
            reader.Setup(v => v.GetValue(FIRST_NAME_ORDINAL_VALUE)).Returns(FIRST_NAME_VALUE);
            reader.Setup(v => v.GetValue(LAST_NAME_ORDINAL)).Returns(LAST_NAME_VALUE);
            reader.Setup(v => v.GetValue(ID_NUMBER_ORDINAL)).Returns(ID_VALUE);
        }

        private void SetUpSimpleFields()
        {
            reader.Setup(v => v.FieldCount).Returns(FIELD_COUNT);

            reader.Setup(v => v.GetName(FIRST_NAME_ORDINAL_VALUE)).Returns(FIRST_NAME);
            reader.Setup(v => v.GetName(LAST_NAME_ORDINAL)).Returns(LAST_NAME);
            reader.Setup(v => v.GetName(ID_NUMBER_ORDINAL)).Returns(ID_NUMBER);
        }

        [Test]
        public void CreateASimpleObjectBasedOnMap()
        {
            CreateReader();

            SetUpSimpleFields();

            SetUpFirstRead();

            reader.Setup(v => v.Read()).Returns(true);

            var result = Creator<SimpleValueObject>.Create(reader.Object);

            result.FirstName.Should().Be(FIRST_NAME_VALUE);
            result.LastName.Should().Be(LAST_NAME_VALUE);
            result.Id.Should().Be(ID_VALUE);
        }

        [Test]
        public void WillCreateASimpleValueListFromReader()
        {
            CreateReader();

            SetUpSimpleFields();

            reader.Setup(v => v.Read()).ReturnsInOrder(true, true, false);

            reader.Setup(v => v.GetValue(FIRST_NAME_ORDINAL_VALUE)).ReturnsInOrder(FIRST_NAME_VALUE, "Han");
            reader.Setup(v => v.GetValue(LAST_NAME_ORDINAL)).ReturnsInOrder(LAST_NAME_VALUE, "Solo");
            reader.Setup(v => v.GetValue(ID_NUMBER_ORDINAL)).ReturnsInOrder(ID_VALUE, 2);

            var list = Creator<SimpleValueObject>.CreateList(reader.Object);

            list.Count.Should().Be(2);

            list[0].FirstName.Should().Be(FIRST_NAME_VALUE);
            list[0].LastName.Should().Be(LAST_NAME_VALUE);
            list[0].Id.Should().Be(ID_VALUE);

            list[1].FirstName.Should().Be("Han");
            list[1].LastName.Should().Be("Solo");
            list[1].Id.Should().Be(2);
        }
    }
}