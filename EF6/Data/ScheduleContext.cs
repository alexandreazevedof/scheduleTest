using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EF6.Data
{
    public class ScheduleContext : DbContext
    {
        //public ScheduleDataContext(DbContextOptions<ScheduleDataContext> options)
        //    : base(options)
        //{ }

        public ScheduleContext(string connString) : base(connString) {

        }
        public virtual DbSet<DefaultSchedule> DefaultSchedule { get; set; }
        public virtual DbSet<CategorizeSettings> CategorizeSettings { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DefaultSchedule>().ToTable("DefaultSchedule");
            modelBuilder.Entity<CategorizeSettings>().ToTable("CategorizeSetting");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");

            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Resource>().ToTable("Resource");

            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ScheduleEquipment>().ToTable("ScheduleEquipments");

            //modelBuilder.Entity<ScheduleEquipment>()
            //    .HasKey(c => new { c.EquipmentId, c.DefaultScheduleId });
        }
        
    }

    public class ScheduleDataContextFactory : IDbContextFactory<ScheduleContext>
    {
        public ScheduleContext Create()
        {
            return new ScheduleContext("Server=(localdb)\\mssqllocaldb;Database=ScheduleWO_TestEF6;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
    public class DefaultSchedule
    {
        public DefaultSchedule()
        {
            this.Equipments = new HashSet<Equipment>();
            this.Customers = new HashSet<Customer>();
            this.Employees = new HashSet<Employee>();
            this.Resources = new HashSet<Resource>();
        }

        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        public string Job { get; set; }
        public DateTime ScheduleFor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime IssueDate { get; set; }
        public string Subject { get; set; }
        public bool AllDay { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public bool Recurrence { get; set; }
        public string RecurrenceRule { get; set; }
        public string Category { get; set; }
        public string EquipmentString { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        public string ResourceString { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public string EmployeeString { get; set; }
        public virtual  ICollection<Employee> Employees { get; set; }
        public string CustomerString { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }

    public class CategorizeSettings
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }

    }

    public class Customer
    {
        public Customer()
        {
            this.DefaultSchedules = new HashSet<DefaultSchedule>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DefaultSchedule> DefaultSchedules { get; set; }
    }

    public class Resource
    {
        public Resource()
        {
            this.DefaultSchedules = new HashSet<DefaultSchedule>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DefaultSchedule> DefaultSchedules { get; set; }
    }

    public class Employee
    {
        public Employee()
        {
            this.DefaultSchedules = new HashSet<DefaultSchedule>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DefaultSchedule> DefaultSchedules { get; set; }
    }

    public class Equipment
    {
        public Equipment()
        {
            this.DefaultSchedules = new HashSet<DefaultSchedule>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DefaultSchedule> DefaultSchedules { get; set; }
    }

    //public class ScheduleEquipment
    //{
    //    public int Id { get; set; }
    //    public int EquipmentId { get; set; }
    //    public int DefaultScheduleId { get; set; }
    //    public Equipment Equipment { get; set; }
    //    public DefaultSchedule DefaultSchedule { get; set; }
    //}

    public class EditParams
    {
        public string key { get; set; }
        public string action { get; set; }
        public List<DefaultSchedule> added { get; set; }
        public List<DefaultSchedule> changed { get; set; }
        public List<DefaultSchedule> deleted { get; set; }
        public DefaultSchedule value { get; set; }
    }
}
