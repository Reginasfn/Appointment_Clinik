using System;
using System.Collections.Generic;

namespace Main_project.Models;

public partial class Appointment
{
    public int IdAppointment { get; set; }

    public int? IdDoctor { get; set; }

    public int? IdMedCard { get; set; }

    public DateOnly? DateAppointment { get; set; }

    public TimeOnly? TimeAppointment { get; set; }

    public string? StatusAppointment { get; set; }

    public virtual Doctor? IdDoctorNavigation { get; set; }

    public virtual User? IdMedCardNavigation { get; set; }
}
