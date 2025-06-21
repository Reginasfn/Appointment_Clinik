using System;
using System.Collections.Generic;

namespace Main_project.Models;

public partial class User
{
    public int IdMedCard { get; set; }

    public string EmailUsers { get; set; } = null!;

    public string PasswordUsers { get; set; } = null!;

    public string RoleIdUsers { get; set; } = null!;

    public string SurnameUsers { get; set; } = null!;

    public string NameUsers { get; set; } = null!;

    public string? PatronymicUsers { get; set; }

    public DateOnly? DateBirth { get; set; }

    public string? MedicalPolicy { get; set; }

    public string? SnilsNumber { get; set; }

    public string? PassportNumber { get; set; }

    public string? AddressRegistration { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Comments { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
