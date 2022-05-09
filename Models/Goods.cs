//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourceProjectMVVMAndEntityFramework.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    
    public partial class Goods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Goods()
        {
            this.Orders_Goods = new HashSet<Orders_Goods>();
        }
    
        public int goodsNumber { get; set; }
        public int userNumber { get; set; }
        public int catNumber { get; set; }
        public string goodsName { get; set; }
        public int goodsCount { get; set; }
        public double goodsCost { get; set; }
        public byte[] goodsPicture { get; set; }
        public string goodsJson { get; set; }
        public JObject JSON {
            get => (JObject)JsonConvert.DeserializeObject(goodsJson);
            set => goodsJson = JsonConvert.SerializeObject(value);
        }

        public virtual Categories Categories { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders_Goods> Orders_Goods { get; set; }
    }
}
