//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Brulliant.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Product = new HashSet<Product>();
        }
    
        public int OrderId { get; set; }
        public string OrderComposition { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public int PickupId { get; set; }
        public string UserFullName { get; set; }
        public int OrderCode { get; set; }
        public int StatusId { get; set; }
    
        public virtual Pickups Pickups { get; set; }
        public virtual Statuses Statuses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Product { get; set; }
    }
}
