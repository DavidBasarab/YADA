using FluentAssertions;
using NUnit.Framework;
using Yada;

namespace YADA.UnitTests.Mapping.Value
{
    [TestFixture]
    [Category("YADA.UnitTests.Mapping.Value")]
    public class NLevelObjectMapping : MappingTest
    {
        private const string HOME_ADDRESS = "Home Address";
        private const int ORDER_COUNT = 53;
        private const string FIRST_NAME = "Chad";
        private const string LAST_NAME = "Morgan";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            CreateReader();
        }

        [Test]
        public void WillMapNLevelObject()
        {
            reader.Setup(v => v.FieldCount).Returns(4);

            reader.Setup(v => v.GetName(0)).Returns("Address_Name");
            reader.Setup(v => v.GetName(1)).Returns("Order_Count");
            reader.Setup(v => v.GetName(2)).Returns("FName");
            reader.Setup(v => v.GetName(3)).Returns("LName");

            reader.Setup(v => v.GetValue(0)).Returns(HOME_ADDRESS);
            reader.Setup(v => v.GetValue(1)).Returns(ORDER_COUNT);
            reader.Setup(v => v.GetValue(2)).Returns(FIRST_NAME);
            reader.Setup(v => v.GetValue(3)).Returns(LAST_NAME);

            reader.Setup(v => v.Read()).Returns(true);

            var result = Creator<NLevelObject>.Create(reader.Object);

            result.NumberOfOrders.Should().Be(ORDER_COUNT);
            result.Location.Should().Be(HOME_ADDRESS);
            result.LevelOne.SimpleValueObject.FirstName.Should().Be(FIRST_NAME);
            result.LevelOne.SimpleValueObject.LastName.Should().Be(LAST_NAME);
        }
    }

    internal class NLevelObjectMapper : Map
    {
        public void CreateMap()
        {
            Mapper.CreateMap<NLevelObject>()
                .Map(v => v.NumberOfOrders, "Order_Count")
                .Map(v => v.Location, "Address_Name")
                .Map(v => v.LevelOne.SimpleValueObject.FirstName, "FName")
                .Map(v => v.LevelOne.SimpleValueObject.LastName, "LName");
        }
    }

    internal class NLevelObject
    {
        public int NumberOfOrders { get; set; }

        public string Location { get; set; }

        public OneLevelObject LevelOne { get; set; }
    }
}