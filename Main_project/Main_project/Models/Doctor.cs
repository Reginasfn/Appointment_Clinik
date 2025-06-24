using System;
using System.Collections.Generic;

namespace Main_project.Models;

public partial class Doctor
{
    public int IdDoctor { get; set; }

    public int IdSpecialty { get; set; }

    public string SurnameDoctor { get; set; } = null!;

    public string NameDoctor { get; set; } = null!;

    public string? PatronymicDoctor { get; set; }

    public string EmailDoctor { get; set; } = null!;

    public string PhoneNumberDoctor { get; set; } = null!;

    public int? MedicalExperience { get; set; }

    public string? CabinetNumber { get; set; }

    public string? StatusWork { get; set; }

    public string? IconDoctor { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Specialty IdSpecialtyNavigation { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public string DisplayIconDoctor => (System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..")) + "\\Resources\\Doctors\\" + ((IconDoctor == null || string.IsNullOrEmpty(IconDoctor)) ? "default_doctor.png" : IconDoctor);
    public string DisplayNameDoctor => $"{SurnameDoctor} {NameDoctor} {PatronymicDoctor}";
    public string DisplayMedExperience => $"Мед. стаж: {MedicalExperience}";
    public string DisplayStatusWork => string.IsNullOrEmpty(StatusWork) ? $"Статус работы: По графику" : $"Статус работы: {StatusWork}";

}