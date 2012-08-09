using System;

namespace YADA.Acceptance.StepDefinations.Values
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public int? PersonID { get; set; }
        public int? StoreID { get; set; }
        public int? TerritoryID { get; set; }
        public string AccountNumber { get; set; }
        public Guid RowGUID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}