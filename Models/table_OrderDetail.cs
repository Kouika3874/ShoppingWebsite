//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project14.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class table_OrderDetail
    {
        public int OrderDetailId { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int Id { get; set; }
        public Nullable<System.Guid> OrderGuid { get; set; }
        public Nullable<bool> IsApproved { get; set; }
    
        public virtual table_Order table_Order { get; set; }
        public virtual table_Product table_Product { get; set; }
    }
}
