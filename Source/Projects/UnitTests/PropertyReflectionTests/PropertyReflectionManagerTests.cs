using System;
using System.Collections.Generic;
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
        private IDictionary<Type, ReflectionHelper> _cacheMock;
        private PropertyReflectionManager _reflectionManager;
        private Type _objectType;
        public MockRepository MockRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockRepository = new MockRepository();

            CreateCacheMock();
        }

        private void CreateCacheMock()
        {
            _cacheMock = MockRepository.StrictMock<IDictionary<Type, ReflectionHelper>>();
        }

        private ReflectionHelper RunTestAction()
        {
            MockRepository.ReplayAll();

            _reflectionManager = new PropertyReflectionManager(_cacheMock);

            var restultHelper = _reflectionManager.GetFromCache(typeof(SampleValue));
            return restultHelper;
        }

        [Test]
        public void IfTypeNotFoundCreatedInCache()
        {
            _objectType = typeof(SampleValue);
            var outHelper = new ReflectionHelper(_objectType);

            _cacheMock.Expect(v => v.TryGetValue(_objectType, out outHelper)).OutRef(outHelper).Return(false);

            var expectedHelper = new ReflectionHelper(_objectType);

            _cacheMock.Expect(v => v.Add(_objectType, expectedHelper));

            var restultHelper = RunTestAction();

            restultHelper.ObjectType.Should().Be(typeof(SampleValue));
        }

        [Test]
        public void ManagerWillReadFromCache()
        {
            _objectType = typeof(SampleValue);
            var outHelper = new ReflectionHelper(_objectType);

            var fakeHelper = new ReflectionHelper(_objectType);

            _cacheMock.Expect(v => v.TryGetValue(_objectType, out outHelper)).OutRef(fakeHelper).Return(true);

            var restultHelper = RunTestAction();

            MockRepository.ReplayAll();

            restultHelper.ObjectType.Should().Be(typeof(SampleValue));
        }
    }
}