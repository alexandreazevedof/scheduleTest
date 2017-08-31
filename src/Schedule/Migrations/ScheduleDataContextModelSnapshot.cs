using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

using EF6.Data;

namespace Schedule.Migrations
{
    [DbContext(typeof(ScheduleContext))]
    partial class ScheduleDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Schedule.Model.CategorizeSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<string>("FontColor");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("CategorizeSetting");
                });

            modelBuilder.Entity("Schedule.Model.DefaultSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllDay");

                    b.Property<string>("Category");

                    b.Property<string>("Customer");

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Employee");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("Equipment");

                    b.Property<DateTime>("IssueDate");

                    b.Property<string>("Job");

                    b.Property<string>("Number");

                    b.Property<bool>("Recurrence");

                    b.Property<string>("RecurrenceRule");

                    b.Property<string>("Resource");

                    b.Property<DateTime>("ScheduleFor");

                    b.Property<DateTime>("StartDate");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.ToTable("DefaultSchedule");
                });

            modelBuilder.Entity("Schedule.Model.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("Schedule.Model.ScheduleEquipment", b =>
                {
                    b.Property<int>("EquipmentId");

                    b.Property<int>("DefaultScheduleId");

                    b.Property<int>("Id");

                    b.HasKey("EquipmentId", "DefaultScheduleId");

                    b.HasIndex("DefaultScheduleId");

                    b.ToTable("ScheduleEquipments");
                });

            modelBuilder.Entity("Schedule.Model.ScheduleEquipment", b =>
                {
                    b.HasOne("Schedule.Model.DefaultSchedule", "DefaultSchedule")
                        .WithMany("ScheduleEquipments")
                        .HasForeignKey("DefaultScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Schedule.Model.Equipment", "Equipment")
                        .WithMany("ScheduleEquipments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
