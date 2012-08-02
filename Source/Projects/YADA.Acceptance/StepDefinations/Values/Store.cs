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

    public class SalesOrderDetail
    {
        public int SalesOrderDetailID { get; set; }
        public int SalesOrderID { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public short OrderQty { get; set; }
        public int ProductID { get; set; }
        public int SpecialOfferID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
        public decimal LineTotal { get; set; }
        public Guid RowGUID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}