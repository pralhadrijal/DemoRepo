﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shopping.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProjectDbEntities : DbContext
    {
        public ProjectDbEntities()
            : base("name=ProjectDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FeatureAd> FeatureAds { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<OurStore> OurStores { get; set; }
        public DbSet<PostAd> PostAds { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Spam> Spams { get; set; }
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<WishList> WishLists { get; set; }
    }
}
