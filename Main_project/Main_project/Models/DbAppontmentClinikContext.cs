using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Main_project.Models;

public partial class DbAppontmentClinikContext : DbContext
{
    public DbAppontmentClinikContext()
    {
    }

    public DbAppontmentClinikContext(DbContextOptions<DbAppontmentClinikContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=root;password=27022006;database=db_appontment_clinik", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.IdAppointment).HasName("PRIMARY");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.IdDoctor, "fk_appointment_doctor_idx");

            entity.HasIndex(e => e.IdMedCard, "fk_appointment_med_card_idx");

            entity.Property(e => e.IdAppointment).HasColumnName("id_appointment");
            entity.Property(e => e.DateAppointment).HasColumnName("date_appointment");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.IdMedCard).HasColumnName("id_med_card");
            entity.Property(e => e.StatusAppointment)
                .HasColumnType("enum('В ожидании','Завершён')")
                .HasColumnName("status_appointment");
            entity.Property(e => e.TimeAppointment)
                .HasColumnType("time")
                .HasColumnName("time_appointment");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdDoctor)
                .HasConstraintName("fk_appointment_doctor");

            entity.HasOne(d => d.IdMedCardNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.IdMedCard)
                .HasConstraintName("fk_appointment_med_card");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("PRIMARY");

            entity.ToTable("doctor");

            entity.HasIndex(e => e.IdSpecialty, "fk_doctor_shedule_idx");

            entity.HasIndex(e => e.IdDoctor, "fk_doctor_shedule_idx1");

            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.CabinetNumber)
                .HasMaxLength(6)
                .HasColumnName("cabinet_number");
            entity.Property(e => e.EmailDoctor)
                .HasMaxLength(100)
                .HasColumnName("email_doctor");
            entity.Property(e => e.IconDoctor)
                .HasMaxLength(50)
                .HasColumnName("icon_doctor");
            entity.Property(e => e.IdSpecialty).HasColumnName("id_specialty");
            entity.Property(e => e.MedicalExperience).HasColumnName("medical_experience");
            entity.Property(e => e.NameDoctor)
                .HasMaxLength(50)
                .HasColumnName("name_doctor");
            entity.Property(e => e.PatronymicDoctor)
                .HasMaxLength(50)
                .HasColumnName("patronymic_doctor");
            entity.Property(e => e.PhoneNumberDoctor)
                .HasMaxLength(11)
                .HasColumnName("phone_number_doctor");
            entity.Property(e => e.StatusWork)
                .HasMaxLength(45)
                .HasColumnName("status_work");
            entity.Property(e => e.SurnameDoctor)
                .HasMaxLength(50)
                .HasColumnName("surname_doctor");

            entity.HasOne(d => d.IdSpecialtyNavigation).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.IdSpecialty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doctor_specialty");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.IdSсhedule).HasName("PRIMARY");

            entity.ToTable("schedule");

            entity.HasIndex(e => e.IdDoctor, "idx_id_doctor");

            entity.Property(e => e.IdSсhedule).HasColumnName("id_sсhedule");
            entity.Property(e => e.DayWeek)
                .HasColumnType("enum('Пн','Вт','Ср','Чт','Пт','Сб','Вс')")
                .HasColumnName("day_week");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("time")
                .HasColumnName("time_end");
            entity.Property(e => e.TimeStart)
                .HasColumnType("time")
                .HasColumnName("time_start");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_schedule_doctor");
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.IdSpecialty).HasName("PRIMARY");

            entity.ToTable("specialty");

            entity.Property(e => e.IdSpecialty).HasColumnName("id_specialty");
            entity.Property(e => e.IconSpecialty)
                .HasMaxLength(50)
                .HasColumnName("icon_specialty");
            entity.Property(e => e.NameSpecialty)
                .HasMaxLength(50)
                .HasColumnName("name_specialty");
            entity.Property(e => e.TimeAccept).HasColumnName("time_accept");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdMedCard).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.IdMedCard).HasColumnName("id_med_card");
            entity.Property(e => e.DateBirth).HasColumnName("date_birth");
            entity.Property(e => e.EmailUsers)
                .HasMaxLength(100)
                .HasColumnName("email_users");
            entity.Property(e => e.MedicalPolicy)
                .HasMaxLength(16)
                .HasColumnName("medical_policy");
            entity.Property(e => e.NameUsers)
                .HasMaxLength(50)
                .HasColumnName("name_users");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(10)
                .HasColumnName("passport_number");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleIdUsers)
                .HasColumnType("enum('Администратор','Пользователь')")
                .HasColumnName("role_id_users");
            entity.Property(e => e.SurnameUsers)
                .HasMaxLength(50)
                .HasColumnName("surname_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
