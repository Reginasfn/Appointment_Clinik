using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Main_project.Scripts
{
    internal class Email_code 
    {
        public const int smtpPort = 587;
        public const string smtpServer = "smtp.mail.ru";

        public const string smtpUsername = "rr-medd@mail.ru";
        public const string smtpPassword = "8z5BeeXoTx8EeHlrRMIF";

        //отправка email (кому, тема, текст)
        public static void SendMessage(string to, string title, string message)
        {
            using SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            using MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(smtpUsername);
            mailMessage.To.Add(to);

            var logoAttachment = new Attachment("C:\\Users\\Регина\\Desktop\\repositories\\Appointment__Clinik\\Main_project\\Main_project\\Resources\\image.png");
            
            logoAttachment.ContentId = "logo"; // Это важно - тот же ID, что и в HTML (cid:logo)
            mailMessage.Attachments.Add(logoAttachment);

            mailMessage.Subject = title;
            mailMessage.Body = message;

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex) when (
                ex.Message.Contains("invalid mailbox") ||
                ex.Message.Contains("Mailbox unavailable") ||
                ex.Message.Contains("recipient verification failed")
            )
            {
                MessageBox.Show($"Ошибка: почта {to} не существует или заблокирована.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}");
            }
        }

        //письмо о регистрации
        public static List<string> GenerateRegMessage(DateTime? appointment, string name, string id)
        {
            string formattedDate = appointment.Value.ToString("dd.MM.yyyy");
            string formattedTime = appointment.Value.ToString("HH:mm");

            string body = $@"
            <!DOCTYPE html>
            <html>
                <head>
                    <style>
                        body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; }}
                        .header {{ 
                            background-color: #4a8bfc; 
                            text-align: center; 
                            border-radius: 10px 10px 0px 0px;
                        }}
                        .logo-container {{
                            display: inline-flex;
                            flex-direction: column;
                            align-items: center;
                            gap: 15px;
                            margin-bottom: 15px;
                        }}
                    
                        .content {{ 
                            padding: 25px;
                            background: #ffffff;
                        }}
                        .code {{ 
                            font-size: 28px; 
                            font-weight: bold; 
                            color: #4a8bfc;
                            text-align: center;
                            margin: 25px 0;
                            padding: 20px;
                            background: #f5f9ff;
                            border-radius: 5px;
                            border: 2px dashed #4a8bfc;
                            letter-spacing: 3px;
                        }}
                        .footer {{ 
                            margin-top: 30px;
                            padding-top: 20px;
                            border-top: 1px solid #eee;
                            font-size: 13px;
                            color: #777;
                            text-align: center;
                        }}
                        .note {{
                            font-size: 14px;
                            color: #666;
                            margin-top: 20px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <div class='logo-container'>
                            <img src='cid:logo' style='height: 300px; width: 300px;' alt='R-Med icon'>
                        </div>
                    </div>
    
                    <div class='content'>
                        <h1 style='color: #4a8bfc; text-align: center;'>Добро пожаловать в нашу клинику!</h1>
                        <h1>Уважаемый(ая) {name}!</h1>
                        <h3>Мы рады сообщить, что вы успешно прикреплены к нашей клинике. Теперь вы можете 
                                записываться на прием к нашим специалистам в любое удобное время.</h3>
        
                        <div style='background-color: #e8f4ff; padding: 15px; border-radius: 6px; margin: 20px 0;'>
                            <p style='margin: 5px 0;'><strong>Ваш номер медицинской карты:</strong> {id}</p>
                            <p style='margin: 5px 0;'><strong>Дата прикрепления:</strong> {DateTime.Now.ToShortDateString()}</p>
                        </div>
                    </div>
    
                    <div class='footer'>
                        <h3>С уважением,<br><strong>❤ Команда R-Med ❤</strong></h3>
                        <p>Обратная связь: <a href='mailto:rr-medd@mail.ru' style='color: #4a8bfc;'>rr-meddp@mail.ru</a></p>
                        <p style='margin-top: 10px;'>{formattedDate} в {formattedTime}</p>
                    </div>
                </body>
            </html>";

            string title = $"Успешное прикрепление к клинике!";

            return new List<string> { title, body };
        }

        //письмо с кодом подтверждения GenerateVerificateMessageAdmin
        public static List<string> GenerateVerificateMessageAdmin(DateTime? appointment, string verificationCode)
        {
            string formattedDate = appointment.Value.ToString("dd.MM.yyyy");
            string formattedTime = appointment.Value.ToString("HH:mm");

            string body = $"Администратор, добро пожаловать!\r\n\r\n" +
                          $"Код подтверждения: {verificationCode}\r\n\r\n\r\n";

            string title = $"Ваш код подтверждения R-Med";

            return new List<string> { title, body };
        }
        public static List<string> GenerateVerificateMessage(DateTime? appointment, string verificationCode)
        {
            string formattedDate = appointment.Value.ToString("dd.MM.yyyy");
            string formattedTime = appointment.Value.ToString("HH:mm");

            string body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; }}
                    .header {{ 
                        background-color: #4a8bfc; 
                        text-align: center; 
                        border-radius: 10px 10px 0px 0px;
                    }}
                    .logo-container {{
                        display: inline-flex;
                        flex-direction: column;
                        align-items: center;
                        gap: 15px;
                        margin-bottom: 15px;
                    }}
                    .logo-text {{
                        color: white;
                        font-size: 24px;
                        font-weight: bold;
                    }}
                    .content {{ 
                        padding: 25px;
                        background: #ffffff;
                    }}
                    .code {{ 
                        font-size: 28px; 
                        font-weight: bold; 
                        color: #4a8bfc;
                        text-align: center;
                        margin: 25px 0;
                        padding: 20px;
                        background: #f5f9ff;
                        border-radius: 5px;
                        border: 2px dashed #4a8bfc;
                        letter-spacing: 3px;
                    }}
                    .footer {{ 
                        margin-top: 30px;
                        padding-top: 20px;
                        border-top: 1px solid #eee;
                        font-size: 13px;
                        color: #777;
                        text-align: center;
                    }}
                    .note {{
                        font-size: 14px;
                        color: #666;
                        margin-top: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <div class='logo-container'>
                        <img src='cid:logo' style='height: 300px; width: 300px;' alt='R-Med icon'>
                    </div>
                </div>
    
                <div class='content'>
                    <h1 style='color: #4a8bfc; text-align: center;'>Код подтверждения</h1>
                    <h1>Уважаемый пользователь!</h1>
                    <h3>Для записи на приём введите следующий код подтверждения:</h3>
        
                    <div class='code'>{verificationCode}</div>
        
                    <p class='note'>Код действителен в течение 5 минут. Никому не сообщайте этот код.</p>
                </div>
    
                <div class='footer'>
                    <h3>С уважением,<br><strong>❤ Команда R-Med ❤</strong></h3>
                    <p>Обратная связь: <a href='mailto:rr-medd@mail.ru' style='color: #4a8bfc;'>rr-meddp@mail.ru</a></p>
                    <p style='margin-top: 10px;'>{formattedDate} в {formattedTime}</p>
                </div>
            </body>
            </html>";

            string title = $"Ваш код подтверждения R-Med";

            return new List<string> { title, body };
        }

        //генерация кода подтверждения
        private static readonly Random _random = new Random();
        public static string GenerateCode()
        {
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                int randomNumber = _random.Next(0, 9);
                code += randomNumber.ToString();
            }
            return code;
        }
    }
}
