using System;
using System.Collections.Generic;

namespace Main_project.Models;

public partial class Specialty
{
    public int IdSpecialty { get; set; }

    public string NameSpecialty { get; set; } = null!;

    public int? TimeAccept { get; set; }

    public string? IconSpecialty { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public string DisplayIconSpecialty => (System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..")) + "\\Resources\\" + ((IconSpecialty == null || string.IsNullOrEmpty(IconSpecialty)) ? null : IconSpecialty);
}
