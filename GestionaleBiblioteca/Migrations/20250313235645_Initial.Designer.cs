﻿// <auto-generated />
using System;
using GestionaleBiblioteca.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestionaleBiblioteca.Migrations
{
    [DbContext(typeof(GestionaleBibliotecaDbContext))]
    [Migration("20250313235645_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GestionaleBiblioteca.Models.Libro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Autore")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("Disponibile")
                        .HasColumnType("bit");

                    b.Property<string>("Genere")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PercorsoImmagineCopertina")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Prezzo")
                        .HasColumnType("float");

                    b.Property<string>("Titolo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Libri");
                });

            modelBuilder.Entity("GestionaleBiblioteca.Models.Prestito", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataPrestito")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataRestituzione")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataRestituzioneEffettiva")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailUtente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LibroId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NomeUtente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LibroId");

                    b.ToTable("Prestiti");
                });

            modelBuilder.Entity("GestionaleBiblioteca.Models.Prestito", b =>
                {
                    b.HasOne("GestionaleBiblioteca.Models.Libro", "Libro")
                        .WithMany("Prestiti")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Libro");
                });

            modelBuilder.Entity("GestionaleBiblioteca.Models.Libro", b =>
                {
                    b.Navigation("Prestiti");
                });
#pragma warning restore 612, 618
        }
    }
}
