using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IEmailService {
    Task SendEmail(string to, string subject, string body, bool isHtml = false);
}
