﻿using FluentAssertions;
using NUnit.Framework;
using Yada;

namespace YADA.UnitTests.Mapping.Value
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
        private const int SECOND_PERSON_ID_VALUE = 15979;
        private const int SECOND_COMPOSITE_ID_VALUE = 859849;
        private const string SECOND_LAST_NAME_VALUE = "Wilson";
        private const string SECOND_FIRST_NAME_VALUE = "Woodrow";
        private const string SECOND_HOUSE_VALUE = "White House";

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            CreateReader();

            SetUpGetOrdinalValues();
        }

        [Test]
        public void WillMapAOneLevelPropertyObject()
        {
            reader.Setup(v => v.Read()).Returns(true);

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

        [Test]
        public void WillMapAOneLevelPropertyObjectList()
        {
            reader.Setup(v => v.Read()).ReturnsInOrder(true, true, false);

            reader.Setup(v => v.GetValue(0)).ReturnsInOrder(SECOND_PERSON_ID_VALUE, PERSON_ID_VALUE);
            reader.Setup(v => v.GetValue(1)).ReturnsInOrder(SECOND_COMPOSITE_ID_VALUE, COMPOSITE_ID_VALUE);
            reader.Setup(v => v.GetValue(2)).ReturnsInOrder(SECOND_LAST_NAME_VALUE, LAST_NAME_VALUE);
            reader.Setup(v => v.GetValue(3)).ReturnsInOrder(SECOND_FIRST_NAME_VALUE, FIRST_NAME_VALUE);
            reader.Setup(v => v.GetValue(4)).ReturnsInOrder(SECOND_HOUSE_VALUE, HOUSE_VALUE);

            var list = Creator<OneLevelObject>.CreateList(reader.Object);

            list.Count.Should().Be(2);

            // First Item
            list[0].Id.Should().Be(SECOND_COMPOSITE_ID_VALUE);
            list[0].Name.Should().Be(SECOND_HOUSE_VALUE);
            list[0].SimpleValueObject.Id.Should().Be(SECOND_PERSON_ID_VALUE);
            list[0].SimpleValueObject.FirstName.Should().Be(SECOND_FIRST_NAME_VALUE);
            list[0].SimpleValueObject.LastName.Should().Be(SECOND_LAST_NAME_VALUE);

            // Second Item
            list[1].Id.Should().Be(COMPOSITE_ID_VALUE);
            list[1].Name.Should().Be(HOUSE_VALUE);
            list[1].SimpleValueObject.Id.Should().Be(PERSON_ID_VALUE);
            list[1].SimpleValueObject.FirstName.Should().Be(FIRST_NAME_VALUE);
            list[1].SimpleValueObject.LastName.Should().Be(LAST_NAME_VALUE);
        }

        private void SetUpGetOrdinalValues()
        {
            reader.Setup(v => v.FieldCount).Returns(5);

            reader.Setup(v => v.GetName(0)).Returns("PersonId");
            reader.Setup(v => v.GetName(1)).Returns("CompositeId");
            reader.Setup(v => v.GetName(2)).Returns("LAST_NAME");
            reader.Setup(v => v.GetName(3)).Returns("FirstName");
            reader.Setup(v => v.GetName(4)).Returns("HouseName");
        }
    }
}