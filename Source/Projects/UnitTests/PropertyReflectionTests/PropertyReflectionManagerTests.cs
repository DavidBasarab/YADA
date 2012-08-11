using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Rhino.Mocks;
using UnitTests.Values;
using YADA.PropertyReflection;

namespace UnitTests.PropertyReflectionTests
{
    [TestFixture]
    [Category("PropertyReflectionTests")]
    public class PropertyReflectionManagerTests
    {
        private IDictionary<Type, object> _cacheMock;
        private PropertyReflectionManager _reflectionManager;
        private Type _objectType;
        public MockRepository MockRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockRepository = new MockRepository();
        }

        private void CreateCacheMock()
        {
            _cacheMock = MockRepository.StrictMock<IDictionary<Type, object>>();
        }

        private void CreateNonMockedReflectionManager()
        {
            _reflectionManager = new PropertyReflectionManager(null);
        }

        private ReflectionHelper<SampleValue> RunTestAction()
        {
            MockRepository.ReplayAll();

            _reflectionManager = new PropertyReflectionManager(_cacheMock);

            var restultHelper = _reflectionManager.GetFromCache<SampleValue>();

            return restultHelper;
        }

        [Test]
        public void CanSetPropertiesThroughReflectionHelper()
        {
            CreateNonMockedReflectionManager();

            var helper = _reflectionManager.GetFromCache<ReflectionValueTester>();

            var tester = new ReflectionValueTester();

            var testingID = new Guid("E4ECCBD8-42EA-4451-A612-0581EDCEF6C7");

            foreach(var propertySetting in helper.Properties)
            {
                if (propertySetting.PropertyName == "FirstName") propertySetting.SetProperty(tester, "David");
                if (propertySetting.PropertyName == "LastName") propertySetting.SetProperty(tester, "Basarab");
                if (propertySetting.PropertyName == "ID") propertySetting.SetProperty(tester, 17);
                if (propertySetting.PropertyName == "NetWorth") propertySetting.SetProperty(tester, 150.36m);
                if (propertySetting.PropertyName == "Height") propertySetting.SetProperty(tester, 14.1);
                if (propertySetting.PropertyName == "UniqueID") propertySetting.SetProperty(tester, testingID);
                if (propertySetting.PropertyName == "BirthDate") propertySetting.SetProperty(tester, DateTime.Parse("04/17/2011"));
            }

            tester.FirstName.Should().Be("David");
            tester.LastName.Should().Be("Basarab");
            tester.ID.Should().Be(17);
            tester.NetWorth.Should().Be(150.36m);
            tester.Height.Should().BeInRange(14.1, 14.2);
            tester.UniqueID.Should().Be(testingID);
            tester.BirthDate.Should().Be(DateTime.Parse("04/17/2011"));
        }

        [Test]
        public void IfTypeNotFoundCreatedInCache()
        {
            CreateCacheMock();

            _objectType = typeof(SampleValue);
            object outHelper = new ReflectionHelper<SampleValue>(_objectType);

            _cacheMock.Expect(v => v.TryGetValue(_objectType, out outHelper)).OutRef(outHelper).Return(false);

            var expectedHelper = new ReflectionHelper<SampleValue>(_objectType);

            _cacheMock.Expect(v => v.Add(_objectType, expectedHelper));

            var restultHelper = RunTestAction();

            restultHelper.ObjectType.Should().Be(typeof(SampleValue));
        }

        [Test]
        public void ManagerWillReadFromCache()
        {
            CreateCacheMock();

            _objectType = typeof(SampleValue);

            object outHelper = new ReflectionHelper<SampleValue>(_objectType);

            object fakeHelper = new ReflectionHelper<SampleValue>(_objectType);

            _cacheMock.Expect(v => v.TryGetValue(_objectType, out outHelper)).OutRef(fakeHelper).Return(true);

            var restultHelper = RunTestAction();

            MockRepository.ReplayAll();

            restultHelper.ObjectType.Should().Be(typeof(SampleValue));
        }

        [Test]
        public void ReflectionHelperWillHaveACollectionOfProperties()
        {
            CreateNonMockedReflectionManager();

            var helper = _reflectionManager.GetFromCache<ReflectionValueTester>();

            helper.Properties.Count().Should().Be(7);
        }
    }
}