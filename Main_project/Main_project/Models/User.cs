using System;
using System.Collections.Generic;

namespace Main_project.Models;

public partial class User
{
    public int IdMedCard { get; set; }

    public string EmailUsers { get; set; } = null!;

    public string RoleIdUsers { get; set; } = null!;

    public string SurnameUsers { get; set; } = null!;

    public string NameUsers { get; set; } = null!;

    public DateOnly? DateBirth { get; set; }

    public string? MedicalPolicy { get; set; }

    public string? PassportNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
