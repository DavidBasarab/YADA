using FluentAssertions;
using NUnit.Framework;
using Yada;

namespace YADA.UnitTests.Mapping
{
    [TestFixture]
    [Category("YADA.UnitTests.Mapping")]
    public class ReadExternalAssemblies
    {
        [Test]
        public void WillCallExternalMapsOnYadaStart()
        {
            Yada.Yada.Start();

            TestMapper.CalledInTest.Should().BeTrue();
        }
    }

    internal class TestMapper : Map
    {
        public static bool CalledInTest { get; set; }

        public static void Reset()
        {
            CalledInTest = false;
        }

        public void CreateMap()
        {
            CalledInTest = true;
        }
    }
}