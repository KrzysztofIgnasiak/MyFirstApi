﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<TradeNote> TradeNotes { get; set; }
        public DbSet<ContactPerson> ContactPeople{ get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().Property(u => u.NameOfUser).HasMaxLength(50);
            builder.Entity<ApplicationUser>().Property(u => u.Surname).HasMaxLength(50);

            //setting max lenght of company columns
            builder.Entity<Company>().Property(u => u.Name).HasMaxLength(50);
            builder.Entity<Company>().Property(u => u.Address).HasMaxLength(150);
            builder.Entity<Company>().Property(u => u.City).HasMaxLength(150);

            //setting properties of company columns
            builder.Entity<Company>().Property(u => u.Name).IsRequired();
            builder.Entity<Company>().Property(u => u.Nip).IsRequired();

            builder.Entity<Industry>().Property(u => u.Name).IsRequired();
            builder.Entity<Industry>().Property(u => u.Name).HasMaxLength(50);

            // builder.Entity<Industry>().HasMany(i => i.Companies).WithOptional(c => c.IndustryType);
            
            //setting trade notes
            builder.Entity<TradeNote>().Property(u => u.Text).IsRequired();
            builder.Entity<Company>().HasMany(c => c.TradeNotes).WithOptional(t => t.CompanyId);

            //setting max lenght of Contact Person columns
            builder.Entity<ContactPerson>().Property(u => u.Name).HasMaxLength(50);
            builder.Entity<ContactPerson>().Property(u => u.Surname).HasMaxLength(50);
            builder.Entity<ContactPerson>().Property(u => u.Mail).HasMaxLength(50);
            builder.Entity<ContactPerson>().Property(u => u.Position).HasMaxLength(100);

            //setting properties of contact Person columns
            builder.Entity<ContactPerson>().Property(u => u.Name).IsRequired();
            builder.Entity<ContactPerson>().Property(u => u.Surname).IsRequired();
            builder.Entity<ContactPerson>().Property(u => u.Mail).IsRequired();
            builder.Entity<ContactPerson>().Property(u => u.Phone).IsOptional();

            builder.Entity<Company>().HasMany(c => c.ContactPeople).WithOptional(t => t.CompanyId);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}