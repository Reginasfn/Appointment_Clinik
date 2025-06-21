using System;
using System.Collections.Generic;

namespace Main_project.Models;

public partial class Schedule
{
    public int IdSсhedule { get; set; }

    public int IdDoctor { get; set; }

    public string DayWeek { get; set; } = null!;

    public TimeOnly TimeStart { get; set; }

    public TimeOnly TimeEnd { get; set; }

    public virtual Doctor IdDoctorNavigation { get; set; } = null!;
}
