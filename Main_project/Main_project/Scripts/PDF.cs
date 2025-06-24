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
                    gfx.DrawString("Если ВЫ не можете прийти на приём в указанное время, \nсообщите об этом по телефону: +7 (347) 201-10-86",
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
        public static void CreateTodaysAppointmentsPdf(string doctorFullName)
        {
            using var db = new DbAppontmentClinikContext();
            string clinicAddress = "г. Уфа, ул. Ленина, 75";

            // Получаем список сегодняшних записей с датой рождения
            var appointments = GetDoctorAppointmentsWithBirthDate(doctorFullName);

            string filePath = @"C:\Users\Регина\Desktop\TodaysAppointments.pdf";

            try
            {
                using (var document = new PdfDocument())
                {
                    // Настройки шрифтов
                    var headerFont = new XFont("Segoe UI", 16, XFontStyle.Bold);
                    var titleFont = new XFont("Segoe UI", 18, XFontStyle.Bold);
                    var infoFont = new XFont("Segoe UI", 12, XFontStyle.Italic);
                    var tableHeaderFont = new XFont("Segoe UI", 12, XFontStyle.Bold);
                    var tableContentFont = new XFont("Segoe UI", 11);
                    var footerFont = new XFont("Segoe UI", 9);
                    var smallInfoFont = new XFont("Segoe UI", 8, XFontStyle.Italic);

                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    double pageWidth = page.Width;
                    double yPosition = 30; // Начальная позиция
                    const double margin = 40;

                    // Заголовок документа
                    gfx.DrawString("Медицинский центр R-Med", headerFont, XBrushes.Black,
                        new XRect(0, yPosition, pageWidth, 0),
                        XStringFormats.TopCenter);
                    yPosition += 30;

                    // Заголовок
                    string today = DateTime.Today.ToString("dd.MM.yyyy");
                    gfx.DrawString($"Расписание приёмов на {today}", titleFont, XBrushes.Black,
                        new XRect(0, yPosition, pageWidth, 0),
                        XStringFormats.TopCenter);
                    yPosition += 35;

                    // Важная информация
                    gfx.DrawString("Приём, за исключением экстренных больных, осуществляется строго по записи!", infoFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    gfx.DrawString($"В случае записи на приём, талон и документы должны быть заранее переданы врачу.", infoFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 20;

                    gfx.DrawString("В случае превышения числа экстренных больных возможен «сдвиг» во времени приёма.", infoFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 30;

                    // Информация о враче
                    gfx.DrawString($"Врач: {doctorFullName}", titleFont, XBrushes.Black,
                        new XRect(margin, yPosition, pageWidth - 2 * margin, 0),
                        XStringFormats.TopLeft);
                    yPosition += 30;

                    // Создаем таблицу
                    double tableWidth = pageWidth - 2 * margin;
                    double col1Width = tableWidth * 0.15;  // Время
                    double col2Width = tableWidth * 0.45;  // ФИО
                    double col3Width = tableWidth * 0.4;    // Дата рождения

                    // Заголовки таблицы
                    double tableYStart = yPosition;
                    gfx.DrawString("Время", tableHeaderFont, XBrushes.Black,
                        new XRect(margin, yPosition, col1Width, 20),
                        XStringFormats.TopLeft);
                    gfx.DrawString("Пациент (Ф.И.)", tableHeaderFont, XBrushes.Black,
                        new XRect(margin + col1Width, yPosition, col2Width, 20),
                        XStringFormats.TopLeft);
                    gfx.DrawString("Дата рождения", tableHeaderFont, XBrushes.Black,
                        new XRect(margin + col1Width + col2Width, yPosition, col3Width, 20),
                        XStringFormats.TopLeft);
                    yPosition += 25;

                    // Линия под заголовком
                    gfx.DrawLine(new XPen(XColors.Black, 1),
                        margin, yPosition,
                        margin + tableWidth, yPosition);
                    yPosition += 10;

                    // Заполняем таблицу данными
                    if (appointments.Any())
                    {
                        foreach (var app in appointments)
                        {
                            // Проверяем, не вышли ли за пределы страницы
                            if (yPosition > page.Height - 70) // Оставляем место для футера
                            {
                                // Создаем новую страницу
                                page = document.AddPage();
                                gfx = XGraphics.FromPdfPage(page);
                                yPosition = 30;

                                // Повторяем заголовок на новой странице
                                gfx.DrawString("Медицинский центр R-Med", headerFont, XBrushes.Black,
                                    new XRect(0, yPosition, pageWidth, 0),
                                    XStringFormats.TopCenter);
                                yPosition += 30;

                                gfx.DrawString($"Расписание приёмов на {today} (продолжение)", titleFont, XBrushes.Black,
                                    new XRect(0, yPosition, pageWidth, 0),
                                    XStringFormats.TopCenter);
                                yPosition += 35;

                                // Заголовки таблицы на новой странице
                                gfx.DrawString("Время", tableHeaderFont, XBrushes.Black,
                                    new XRect(margin, yPosition, col1Width, 20),
                                    XStringFormats.TopLeft);
                                gfx.DrawString("Пациент (Ф.И.)", tableHeaderFont, XBrushes.Black,
                                    new XRect(margin + col1Width, yPosition, col2Width, 20),
                                    XStringFormats.TopLeft);
                                gfx.DrawString("Дата рождения", tableHeaderFont, XBrushes.Black,
                                    new XRect(margin + col1Width + col2Width, yPosition, col3Width, 20),
                                    XStringFormats.TopLeft);
                                yPosition += 25;

                                gfx.DrawLine(new XPen(XColors.Black, 1),
                                    margin, yPosition,
                                    margin + tableWidth, yPosition);
                                yPosition += 10;
                            }

                            // Содержимое таблицы
                            gfx.DrawString(app.Time, tableContentFont, XBrushes.Black,
                                new XRect(margin, yPosition, col1Width, 20),
                                XStringFormats.TopLeft);
                            gfx.DrawString(app.PatientName, tableContentFont, XBrushes.Black,
                                new XRect(margin + col1Width, yPosition, col2Width, 20),
                                XStringFormats.TopLeft);
                            gfx.DrawString(app.BirthDate, tableContentFont, XBrushes.Black,
                                new XRect(margin + col1Width + col2Width, yPosition, col3Width, 20),
                                XStringFormats.TopLeft);
                            yPosition += 20;

                            // Линия между записями
                            gfx.DrawLine(new XPen(XColors.LightGray, 0.5),
                                margin, yPosition,
                                margin + tableWidth, yPosition);
                            yPosition += 5;
                        }
                    }
                    else
                    {
                        gfx.DrawString("На сегодня записей нет", tableContentFont, XBrushes.Black,
                            new XRect(margin, yPosition, tableWidth, 20),
                            XStringFormats.TopCenter);
                        yPosition += 25;
                    }

                    // Футер
                    yPosition += 20;
                    gfx.DrawLine(new XPen(XColors.Black, 0.5),
                        margin, yPosition,
                        margin + tableWidth, yPosition);
                    yPosition += 10;

                    gfx.DrawString($"Адрес: {clinicAddress}", footerFont, XBrushes.Black,
                        new XRect(margin, yPosition, tableWidth, 20),
                        XStringFormats.TopLeft);
                    yPosition += 15;

                    gfx.DrawString("Телефон регистратуры: +7 (347) 201-10-86", footerFont, XBrushes.Black,
                        new XRect(margin, yPosition, tableWidth, 20),
                        XStringFormats.TopLeft);
                    yPosition += 15;

                    // Конфиденциальность
                    gfx.DrawString("В целях соблюдения конфиденциальности указаны только первые буквы Ф.И.", smallInfoFont, XBrushes.Gray,
                        new XRect(margin, yPosition, tableWidth, 20),
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

        public static List<(string Time, string PatientName, string BirthDate)> GetDoctorAppointmentsWithBirthDate(string doctorFullName)
        {
            using var db = new DbAppontmentClinikContext();

            var nameParts = doctorFullName.Split(' ');
            string surname = nameParts[0];
            string name = nameParts[1];
            string patronymic = nameParts.Length > 2 ? nameParts[2] : null;
            var doctor = db.Doctors
                .Where(d => d.SurnameDoctor == surname &&
                           d.NameDoctor == name &&
                           (patronymic == null || d.PatronymicDoctor == patronymic))
                .FirstOrDefault();

            if (doctor == null)
            {
                MessageBox.Show("Врач не найден");
                return new List<(string, string, string)>();
            }
            var today = DateOnly.FromDateTime(DateTime.Today);
            var appointments = db.Appointments
                .Where(a => a.IdDoctor == doctor.IdDoctor &&
                           a.DateAppointment == today)
                .Include(a => a.IdMedCardNavigation)
                .OrderBy(a => a.TimeAppointment)
                .ToList();

            var result = new List<(string Time, string PatientName, string BirthDate)>();

            foreach (var app in appointments)
            {
                if (app.IdMedCardNavigation != null && app.TimeAppointment.HasValue)
                {
                    string time = app.TimeAppointment.Value.ToString("HH\\:mm");
                    string patientName = $"{app.IdMedCardNavigation.SurnameUsers[0]}. {app.IdMedCardNavigation.NameUsers[0]}.";
                    string birthDate = app.IdMedCardNavigation.DateBirth?.ToString("dd.MM.yyyy") ?? "не указана";

                    result.Add((time, patientName, birthDate));
                }
            }

            return result;
        }
    }
}
