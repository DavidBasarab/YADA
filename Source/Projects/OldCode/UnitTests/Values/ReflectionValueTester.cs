using System;

namespace UnitTests.Values
{
    public class ReflectionValueTester : SampleValue
    {
        public DateTime BirthDate { get; set; }
        public int ID { get; set; }
        public decimal NetWorth { get; set; }
        public double Height { get; set; }
        public Guid UniqueID { get; set; }
    }
}