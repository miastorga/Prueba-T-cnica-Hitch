﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AsientosContrablesApi.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20231208184714_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AsientosContrablesApi.Models.AsientoContable", b =>
                {
                    b.Property<Guid>("Reference")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Memo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReferenceDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TaxDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Reference");

                    b.ToTable("AsientosContables");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.JournalEntryLines", b =>
                {
                    b.Property<Guid>("Reference1")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("AsientoContableId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Credit")
                        .HasColumnType("float");

                    b.Property<double>("Debit")
                        .HasColumnType("float");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<string>("LineMemo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Reference1");

                    b.HasIndex("AsientoContableId");

                    b.ToTable("JournalEntryLines");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.Proceso", b =>
                {
                    b.Property<Guid>("ProcessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Failed")
                        .HasColumnType("int");

                    b.Property<int>("Items")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("Success")
                        .HasColumnType("int");

                    b.HasKey("ProcessId");

                    b.ToTable("Procesos");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.Registro", b =>
                {
                    b.Property<Guid>("RegistroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AsientoContableReference")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Error")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberSap1")
                        .HasColumnType("int");

                    b.Property<int>("NumberSap2")
                        .HasColumnType("int");

                    b.Property<Guid>("ProcessId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("RegistroId");

                    b.HasIndex("AsientoContableReference");

                    b.HasIndex("ProcessId");

                    b.ToTable("Registros");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.JournalEntryLines", b =>
                {
                    b.HasOne("AsientosContrablesApi.Models.AsientoContable", "AsientoContable")
                        .WithMany("JournalEntryLines")
                        .HasForeignKey("AsientoContableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AsientoContable");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.Registro", b =>
                {
                    b.HasOne("AsientosContrablesApi.Models.AsientoContable", "AsientoContable")
                        .WithMany("Registros")
                        .HasForeignKey("AsientoContableReference")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AsientosContrablesApi.Models.Proceso", "Proceso")
                        .WithMany("Registros")
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AsientoContable");

                    b.Navigation("Proceso");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.AsientoContable", b =>
                {
                    b.Navigation("JournalEntryLines");

                    b.Navigation("Registros");
                });

            modelBuilder.Entity("AsientosContrablesApi.Models.Proceso", b =>
                {
                    b.Navigation("Registros");
                });
#pragma warning restore 612, 618
        }
    }
}
