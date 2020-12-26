using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Management.Models;

namespace Management.Data
{

	public class OBDContext : DbContext
	{

		public OBDContext(DbContextOptions<OBDContext> options) : base(options)
		{
		}

        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<LogEntry> LogEntry { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<CarBrand> CarBrand { get; set; }
        public virtual DbSet<CarModel> CarModel { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Configuration> Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Car>(entity =>
            {

                entity.HasKey(e => e.CarId);

                entity.Property(e => e.Year)
                    .HasColumnType("date");

                entity.HasOne(d => d.CarBrand)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarBrandId)
                    .HasConstraintName("FK_Car_BrandId");

                entity.HasOne(d => d.CarModel)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarModelId)
                    .HasConstraintName("FK_Car_ModelId");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Car_PersonId");

            });

            modelBuilder.Entity<CarBrand>(entity =>
            {
                entity.HasKey(e => e.CarBrandId);
                entity.Property(e => e.Name)
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CarModel>(entity =>
            {
                entity.HasKey(e => e.CarModelId);
                entity.Property(e => e.Name)
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogEntry>(entity =>
            {

                entity.HasKey(e => e.LogEntryId);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.LogEntries)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_LogEntry_SessionId");

            });

            modelBuilder.Entity<Session>(entity =>
            {

                entity.HasKey(e => e.SessionId);

                entity.Property(e => e.DateStart)
                    .HasColumnType("datetime");
                entity.Property(e => e.DateStop)
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Session_CarId");
            });

            modelBuilder.Entity<Person>(entity =>
            {

                entity.HasKey(e => e.PersonId);

                entity.Property(e => e.GivenName)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.SurName)
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.HasKey(e => e.ConfigurationId);
            });

            base.OnModelCreating(modelBuilder);

        }

    }
}
