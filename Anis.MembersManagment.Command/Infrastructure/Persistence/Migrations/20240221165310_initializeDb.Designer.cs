﻿// <auto-generated />
using System;
using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Anis.MembersManagment.Command.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240221165310_initializeDb")]
    partial class initializeDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Anis.MembersManagment.Command.Events.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Sequence")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AggregateId", "Sequence")
                        .IsUnique();

                    b.ToTable("Events");

                    b.HasDiscriminator<string>("EventType").HasValue("Event");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Anis.MembersManagment.Command.Infrastructure.Persistence.OutboxMessage", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("Anis.MembersManagment.Command.Events.InvitationAccepted", b =>
                {
                    b.HasBaseType("Anis.MembersManagment.Command.Events.Event");

                    b.Property<string>("Data")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Data");

                    b.HasDiscriminator().HasValue("InvitationAccepted");
                });

            modelBuilder.Entity("Anis.MembersManagment.Command.Events.InvitationCancelled", b =>
                {
                    b.HasBaseType("Anis.MembersManagment.Command.Events.Event");

                    b.Property<string>("Data")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Data");

                    b.HasDiscriminator().HasValue("InvitationCancelled");
                });

            modelBuilder.Entity("Anis.MembersManagment.Command.Events.InvitationRejected", b =>
                {
                    b.HasBaseType("Anis.MembersManagment.Command.Events.Event");

                    b.Property<string>("Data")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Data");

                    b.HasDiscriminator().HasValue("InvitationRejected");
                });

            modelBuilder.Entity("Anis.MembersManagment.Command.Events.InvitationSent", b =>
                {
                    b.HasBaseType("Anis.MembersManagment.Command.Events.Event");

                    b.Property<string>("Data")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Data");

                    b.HasDiscriminator().HasValue("InvitationSent");
                });

            modelBuilder.Entity("Anis.MembersManagment.Command.Infrastructure.Persistence.OutboxMessage", b =>
                {
                    b.HasOne("Anis.MembersManagment.Command.Events.Event", "Event")
                        .WithOne()
                        .HasForeignKey("Anis.MembersManagment.Command.Infrastructure.Persistence.OutboxMessage", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });
#pragma warning restore 612, 618
        }
    }
}
