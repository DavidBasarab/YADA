using System;

namespace YADA.Acceptance.StepDefinations.Values
{
    public class Store
    {
        public int BusinessEntityID { get; set; }
        public string Name { get; set; }
        public int? SalesPersonID { get; set; }
        public string Demographics { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}