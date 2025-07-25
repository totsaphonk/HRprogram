﻿// <auto-generated />
using System;
using HR.LeaveManagement.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HR.LeaveManagement.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250718124509_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.7");

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ManagerID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DepartmentID = 1,
                            Description = "HR Department",
                            Name = "Human Resources"
                        },
                        new
                        {
                            DepartmentID = 2,
                            Description = "Information Technology",
                            Name = "IT"
                        },
                        new
                        {
                            DepartmentID = 3,
                            Description = "Finance Department",
                            Name = "Finance"
                        });
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ActiveStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int?>("DepartmentID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            EmployeeID = 1,
                            ActiveStatus = true,
                            Department = "IT",
                            Email = "john.doe@company.com",
                            FullName = "John Doe",
                            JoinDate = new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "Developer"
                        },
                        new
                        {
                            EmployeeID = 2,
                            ActiveStatus = true,
                            Department = "HR",
                            Email = "jane.smith@company.com",
                            FullName = "Jane Smith",
                            JoinDate = new DateTime(2022, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "HR Manager"
                        },
                        new
                        {
                            EmployeeID = 3,
                            ActiveStatus = true,
                            Department = "Finance",
                            Email = "bob.johnson@company.com",
                            FullName = "Bob Johnson",
                            JoinDate = new DateTime(2021, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "Accountant"
                        });
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.EntitlementRule", b =>
                {
                    b.Property<int>("RuleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("EntitledDays")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LeaveTypeID")
                        .HasColumnType("INTEGER");

                    b.HasKey("RuleID");

                    b.HasIndex("LeaveTypeID");

                    b.ToTable("EntitlementRules");

                    b.HasData(
                        new
                        {
                            RuleID = 1,
                            Condition = "YearsOfService >= 1",
                            EntitledDays = 14,
                            LeaveTypeID = 1
                        },
                        new
                        {
                            RuleID = 2,
                            Condition = "YearsOfService >= 0",
                            EntitledDays = 30,
                            LeaveTypeID = 2
                        },
                        new
                        {
                            RuleID = 3,
                            Condition = "YearsOfService >= 1",
                            EntitledDays = 5,
                            LeaveTypeID = 3
                        });
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.LeaveRequest", b =>
                {
                    b.Property<int>("RequestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ApprovedBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LeaveTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("TEXT");

                    b.HasKey("RequestID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("LeaveTypeID");

                    b.ToTable("LeaveRequests");
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.LeaveType", b =>
                {
                    b.Property<int>("LeaveTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AllowHalfDay")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("RequiresDoc")
                        .HasColumnType("INTEGER");

                    b.HasKey("LeaveTypeID");

                    b.ToTable("LeaveTypes");

                    b.HasData(
                        new
                        {
                            LeaveTypeID = 1,
                            AllowHalfDay = true,
                            Description = "Annual vacation leave",
                            Name = "Annual Leave",
                            RequiresDoc = false
                        },
                        new
                        {
                            LeaveTypeID = 2,
                            AllowHalfDay = true,
                            Description = "Medical leave",
                            Name = "Sick Leave",
                            RequiresDoc = true
                        },
                        new
                        {
                            LeaveTypeID = 3,
                            AllowHalfDay = false,
                            Description = "Personal time off",
                            Name = "Personal Leave",
                            RequiresDoc = false
                        });
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.Employee", b =>
                {
                    b.HasOne("HR.LeaveManagement.Web.Models.Department", null)
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentID");
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.EntitlementRule", b =>
                {
                    b.HasOne("HR.LeaveManagement.Web.Models.LeaveType", "LeaveType")
                        .WithMany("EntitlementRules")
                        .HasForeignKey("LeaveTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LeaveType");
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.LeaveRequest", b =>
                {
                    b.HasOne("HR.LeaveManagement.Web.Models.Employee", "Employee")
                        .WithMany("LeaveRequests")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HR.LeaveManagement.Web.Models.LeaveType", "LeaveType")
                        .WithMany("LeaveRequests")
                        .HasForeignKey("LeaveTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("LeaveType");
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.Department", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.Employee", b =>
                {
                    b.Navigation("LeaveRequests");
                });

            modelBuilder.Entity("HR.LeaveManagement.Web.Models.LeaveType", b =>
                {
                    b.Navigation("EntitlementRules");

                    b.Navigation("LeaveRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
