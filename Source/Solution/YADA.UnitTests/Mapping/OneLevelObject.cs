using Yada;

namespace YADA.UnitTests.Mapping
{
    internal class OneLevelObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SimpleValueObject SimpleValueObject { get; set; }
    }

    internal class OneLevelObjectMap : Map
    {
        public void CreateMap()
        {
            Mapper.CreateMap<OneLevelObject>()
                .Map(v => v.Id, "CompositeId")
                .Map(v => v.Name, "HouseName")
                .Map(v => v.SimpleValueObject.FirstName, "FirstName")
                .Map(v => v.SimpleValueObject.LastName, "LAST_NAME")
                .Map(v => v.SimpleValueObject.Id, "PersonId");
        }
    }
}