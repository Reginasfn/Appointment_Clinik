using System;
using System.Collections.Generic;
using System.Windows;
using Main_project.Models;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Main_project.Scripts
{
    internal class PDF
    {
        public static void CreateAppointmentTicketPdf(string doctorSpecialization, string doctorName, string cabinetNumber, string clinicAddress, string date, string time)
        {
            string filePath = @"C:\Users\Регина\Desktop\AppointmentTicket.pdf";
            try
            {
                using (var document = new PdfDocument())
                {
                    // Узкие настройки шрифтов
                    var headerFont = new XFont("Segoe UI", 12, XFontStyle.Bold);
                    var titleFont = new XFont("Segoe UI", 14, XFontStyle.Bold);
                    var dateTimeFont = new XFont("Segoe UI", 18, XFontStyle.Bold);
                    var dateTimeFont1 = new XFont("Segoe UI", 22, XFontStyle.Bold);
                    var infoFont = new XFont("Segoe UI", 9);
                    var smallInfoFont = new XFont("Segoe UI", 7);

                    // Узкие настройки документа
                    const double margin = 20; // Минимальные отступы
                    const double contentWidth = 300; // Фиксированная ширина контента
                    double yPosition = margin;
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    double centerX = page.Width / 2;

                    // Заголовок документа (компактный в углу)
                    gfx.DrawString("R-Med", headerFont, XBrushes.Black,
                        new XRect(page.Width - margin - 180, yPosition, 50, 20),
                        XStringFormats.TopLeft);
                    yPosition += 30;

                    // Основной текст
                    gfx.DrawString("Вы успешно записаны на приём!", titleFont, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopCenter);
                    yPosition += 30;

                    // Дата и время (в одну строку)
                    string dateTime = $"{date}";
                    gfx.DrawString(dateTime, dateTimeFont1, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopCenter);
                    yPosition += 40;

                    // Дата и время (в одну строку)
                    string Time = $"{time}";
                    gfx.DrawString(Time, dateTimeFont1, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopCenter);
                    yPosition += 50;

                    // Информация о враче (плотная группировка)
                    gfx.DrawString(doctorSpecialization, infoFont, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    gfx.DrawString($"Врач: {doctorName}", infoFont, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    gfx.DrawString($"{cabinetNumber}", dateTimeFont, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopLeft);
                    yPosition += 30;

                    gfx.DrawString($"Адрес: {clinicAddress}", infoFont, XBrushes.Black,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopLeft);
                    yPosition += 25;

                    // Компактный разделитель
                    gfx.DrawLine(new XPen(XColors.Black, 0.5),
                        centerX - contentWidth / 2, yPosition,
                        centerX + contentWidth / 2, yPosition);
                    yPosition += 15;

                    // Компактная контактная информаци
                    gfx.DrawString("Если ВЫ не можете прийти на приём в указанное время, \nсообщите об этом по телефону: +7-900-000-001",
                        smallInfoFont, XBrushes.Gray,
                        new XRect(centerX - contentWidth / 2, yPosition, contentWidth, 0),
                        XStringFormats.TopLeft);

                    // Сохранение документа
                    document.Save(filePath);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            $"PDF с талоном на приём сохранён:\n{filePath}",
                            "Сохранение завершено",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    });
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        $"Ошибка при создании PDF: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }
        public static void CreateTodaysAppointmentsPdf()
        {
            using var db = new DbAppontmentClinikContext();

            string doctorSpecialization = "Терапевт";
            string doctorFullName = "Быкова Лариса Петровна";
            string clinicAddress = "г. Уфа, ул. Ленина, 75";

            // Получаем список сегодняшних записей
            var appointments = GetDoctorAppointments(doctorFullName);

            string filePath = @"C:\Users\Регина\Desktop\TodaysAppointments.pdf";

            try
            {
                using (var document = new PdfDocument())
                {
                    // Настройки шрифтов
                    var headerFont = new XFont("Segoe UI", 16, XFontStyle.Bold);
                    var titleFont = new XFont("Segoe UI", 20, XFontStyle.Bold);
                    var doctorInfoFont = new XFont("Segoe UI", 14, XFontStyle.Bold);
                    var dateFont = new XFont("Segoe UI", 12, XFontStyle.Bold);
                    var appointmentFont = new XFont("Segoe UI", 12);
                    var smallInfoFont = new XFont("Segoe UI", 10);

                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    double pageWidth = page.Width;
                    double yPosition = 40;
                    const double margin = 40;

                    // Заголовок документа
                    gfx.DrawString("R-Med", headerFont, XBrushes.Black,
                        new XRect(-30, yPosition, pageWidth, 0),
                        XStringFormats.TopRight);
                    yPosition += 40;

                    // Заголовок
                    gfx.DrawString("Сегодняшние записи на приём", titleFont, XBrushes.Black,
                        new XRect(0, yPosition, pageWidth, 0),
                        XStringFormats.TopCenter);
                    yPosition += 40;

                    // Дата
                    string today = DateTime.Today.ToString("dd.MM.yyyy");
                    gfx.DrawString($"Дата: {today}", dateFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 30;

                    // Информация о враче
                    gfx.DrawString($"Врач: {doctorFullName}", doctorInfoFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 25;

                    gfx.DrawString($"Специализация: {doctorSpecialization}", doctorInfoFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 40;

                    // Разделитель
                    gfx.DrawLine(new XPen(XColors.Black, 0.5),
                        margin, yPosition,
                        pageWidth - margin, yPosition);
                    yPosition += 20;

                    // Выводим сегодняшние записи
                    if (appointments.Any())
                    {
                        gfx.DrawString("Расписание:", appointmentFont, XBrushes.Black,
                            new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                            XStringFormats.TopLeft);
                        yPosition += 25;

                        foreach (var app in appointments)
                        {
                            gfx.DrawString($"{app.Time} - {app.PatientName}", appointmentFont, XBrushes.Black,
                                new XRect(margin + 20, yPosition, pageWidth - 2 * margin, 0),
                                XStringFormats.TopLeft);
                            yPosition += 20;
                        }
                    }
                    else
                    {
                        gfx.DrawString("На сегодня записей нет", appointmentFont, XBrushes.Black,
                            new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                            XStringFormats.TopLeft);
                        yPosition += 20;
                    }

                    // Разделитель
                    yPosition += 10;
                    gfx.DrawLine(new XPen(XColors.Black, 0.5),
                        margin, yPosition,
                        pageWidth - margin, yPosition);
                    yPosition += 20;

                    // Адрес клиники
                    gfx.DrawString($"Адрес: {clinicAddress}", smallInfoFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    // Контактная информация
                    gfx.DrawString("Телефон для справок: +7-900-000-001", smallInfoFont, XBrushes.Gray,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);

                    document.Save(filePath);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            $"PDF с расписанием на сегодня сохранён:\n{filePath}",
                            "Сохранение завершено",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    });
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        $"Ошибка при создании PDF: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }

        public static List<(string Time, string PatientName)> GetDoctorAppointments(string doctorFullName)
        {
            using var db = new DbAppontmentClinikContext();

            var nameParts = doctorFullName.Split(' ');
            string surname = nameParts[0];
            string name = nameParts[1];
            string patronymic = nameParts.Length > 2 ? nameParts[2] : null;

            // Находим врача по ФИО
            var doctor = db.Doctors
                .Where(d => d.SurnameDoctor == surname &&
                           d.NameDoctor == name &&
                           (patronymic == null || d.PatronymicDoctor == patronymic))
                .FirstOrDefault();

            if (doctor == null)
            {
                MessageBox.Show("Врач не найден");
                return new List<(string, string)>();
            }

            // Получаем сегодняшнюю дату
            var today = DateOnly.FromDateTime(DateTime.Today);

            // Получаем только сегодняшние записи к этому врачу
            var appointments = db.Appointments
                .Where(a => a.IdDoctor == doctor.IdDoctor &&
                           a.DateAppointment == today)
                .Include(a => a.IdMedCardNavigation)
                .OrderBy(a => a.TimeAppointment)
                .ToList();

            var result = new List<(string Time, string PatientName)>();

            foreach (var app in appointments)
            {
                if (app.IdMedCardNavigation != null && app.TimeAppointment.HasValue)
                {
                    // Форматируем время как "00:00"
                    string time = app.TimeAppointment.Value.ToString("hh\\:mm");
                    string patientName = $"{app.IdMedCardNavigation.SurnameUsers} {app.IdMedCardNavigation.NameUsers[0]}";

                    result.Add((time, patientName));
                }
            }

            return result;
        }
    }
}
