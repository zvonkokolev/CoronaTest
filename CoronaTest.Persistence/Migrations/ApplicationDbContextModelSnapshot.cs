﻿// <auto-generated />
using System;
using CoronaTest.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoronaTest.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CampaignTestCenter", b =>
                {
                    b.Property<int>("AvailableInCampaignsId")
                        .HasColumnType("int");

                    b.Property<int>("AvailableTestCentersId")
                        .HasColumnType("int");

                    b.HasKey("AvailableInCampaignsId", "AvailableTestCentersId");

                    b.HasIndex("AvailableTestCentersId");

                    b.ToTable("CampaignTestCenter");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.AuthRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("AuthRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.AuthUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuthUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@htl.at",
                            Password = "OqNdiXS5p4DlTJ+QPhtwpWBxumBm/Ssm/MJ00OK3eKg=eca3178af8210aaa3ac53bd07dc70f76",
                            UserRole = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user@htl.at",
                            Password = "BjUjOLpeXHs/aDZtvMM98eFLex5to0NRBgd/j5sf/pg=31882ca8e570ec55f3e9241680aaa2aa",
                            UserRole = "User"
                        });
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.Campaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.Examination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CampaignId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExaminationAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParticipantId")
                        .HasColumnType("int");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int?>("TestCenterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.HasIndex("ParticipantId");

                    b.HasIndex("TestCenterId");

                    b.ToTable("Examinations");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.Participant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Door")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobilephone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postalcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("SocialSecurityNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Stair")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.TestCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postalcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("SlotCapacity")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TestCenters");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.VerificationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Identifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsInvalidated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParticipantId")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Token")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParticipantId");

                    b.ToTable("VerificationTokens");
                });

            modelBuilder.Entity("CampaignTestCenter", b =>
                {
                    b.HasOne("CoronaTest.Core.Entities.Campaign", null)
                        .WithMany()
                        .HasForeignKey("AvailableInCampaignsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoronaTest.Core.Entities.TestCenter", null)
                        .WithMany()
                        .HasForeignKey("AvailableTestCentersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.Examination", b =>
                {
                    b.HasOne("CoronaTest.Core.Entities.Campaign", "Campaign")
                        .WithMany()
                        .HasForeignKey("CampaignId");

                    b.HasOne("CoronaTest.Core.Entities.Participant", "Participant")
                        .WithMany()
                        .HasForeignKey("ParticipantId");

                    b.HasOne("CoronaTest.Core.Entities.TestCenter", "TestCenter")
                        .WithMany()
                        .HasForeignKey("TestCenterId");

                    b.Navigation("Campaign");

                    b.Navigation("Participant");

                    b.Navigation("TestCenter");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.VerificationToken", b =>
                {
                    b.HasOne("CoronaTest.Core.Entities.Participant", null)
                        .WithMany("Verifications")
                        .HasForeignKey("ParticipantId");
                });

            modelBuilder.Entity("CoronaTest.Core.Entities.Participant", b =>
                {
                    b.Navigation("Verifications");
                });
#pragma warning restore 612, 618
        }
    }
}
