﻿using System.Threading.Tasks;

namespace Backend_Dis_App.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject);
    }
}
