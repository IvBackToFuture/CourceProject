﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourceProjectMVVMAndEntityFramework.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OneStopStoreEntities : DbContext
    {
        private static OneStopStoreEntities _Context = new OneStopStoreEntities();
        public static OneStopStoreEntities GetContext() => _Context;

        public OneStopStoreEntities()
            : base("name=OneStopStoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Orders_Goods> Orders_Goods { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
