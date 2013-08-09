using Yada;

namespace YADA.UnitTests.Mapping.Value
{
    internal class SimpleValueObjectMapping : Map
    {
        public void CreateMap()
        {
            Mapper.CreateMap<SimpleValueObject>()
                .Map(v => v.FirstName, "FIRST_NAME")
                .Map(v => v.LastName, "LAST_NAME")
                .Map(v => v.Id, "ID_NUMBER");
        }
    }
}