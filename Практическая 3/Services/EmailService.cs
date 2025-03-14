﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Windows;
using Практическая_3.Models;

namespace Практическая_3.Services
{
    /// <summary>
    /// Отправляет код подтверждения на электронную почту
    /// </summary>
    /// <param name="smtpServer">Название сервера на который будет поступать запрос</param>
    /// <param name="smtpPort">Порт сервера</param>
    /// <param name="smtpUsername">Почта с которой будет отправляться письмо</param>
    /// <param name="smtpPassword">Пароль для доступа к аккаунту почти с внешнего приложения</param>
    public class EmailService // Класс для отправки кода подтверждения на электронную почту клиента
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }

        /// <summary>
        /// Отправляет само сообщение пользователю на адресс электронной почты
        /// </summary>
        /// <param name="userEmail">Почта на которую юудет отправлено письмо</param>
        /// <param name="code">Код для сброса/авторизации</param>
        public void SendPasswordResetEmail(string userEmail, string code)
        {
            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    client.EnableSsl = true; //подключение протокола шифрования

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpUsername, "ELON MUSK"),
                        Subject = "ПРОВЕРОЧНЫЙ КОД",
                        Body = $"ПЕРСОНАЛЬНЫЙ КОД ДЛЯ ЛУЧШИХ!!!: {code}",
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(userEmail);

                    client.Send(mailMessage);
                }
            }
            catch(Exception e) 
            {
                MessageBox.Show($"Ошибка при отправке письма: {e.Message}");
            }
        }
    }

    public class UserService
    {
        private readonly HospitalProEntities dbContext;
        private readonly EmailService emailService;

        public UserService(HospitalProEntities dbContext, EmailService emailService)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
        }

        /// <summary>
        /// Ищет пользователя в системе
        /// </summary>
        /// <param name="userEmail">Email пользователя по которому происходит поиск</param>
        /// <param name="code">Код для сброса пароля</param>
        public void RequestPasswordReset(string userEmail, int code)
        {
            var patient = dbContext.Patient.FirstOrDefault(x => x.email == userEmail);
            var staff = dbContext.Staff.FirstOrDefault(x => x.email == userEmail);

            if (patient != null)
            {
                emailService.SendPasswordResetEmail(userEmail, code.ToString());
            }
            else if (staff != null)
            {
                emailService.SendPasswordResetEmail(userEmail, code.ToString());
            } else 
            {
                MessageBox.Show("Пользователь не найден");
            }
        }
    }
}
