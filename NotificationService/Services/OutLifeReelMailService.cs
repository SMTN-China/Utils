using Hangfire;
using NotificationService.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace NotificationService.Services
{
    public class OutLifeReelMailService
    {
        public HttpHelp HttpHelp { get; set; }

        public void StartMailTask()
        {

            var config = HttpHelp.RequsetAbp<SettingValue[]>(RequsetMethod.POST, "/api/services/app/Configuration/GetAppConfig", new string[] { "asyncInterval" }, null, false).Result;
            RecurringJob.AddOrUpdate("OutLifeReelMailTask", () => Send(),
                Cron.HourInterval(int.Parse(config.FirstOrDefault(c => c.Name == "asyncInterval").Value)));
        }

        public void StopMailTask()
        {
            RecurringJob.RemoveIfExists("OutLifeReelMailTask");
        }


        public void Send()
        {

            var reelOutLife = HttpHelp.RequsetAbp<PagedResultDto<ReelOutLifeDto>>(RequsetMethod.POST, "/api/services/app/Reel/GetOutLifeReel", new PagedResultRequestMESDto() { SkipCount = 0, MaxResultCount = 10 }, null, false).Result.Items;

            if (reelOutLife == null || reelOutLife.Count == 0)
            {
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SMT-智能仓储系统", ConfigurationManager.AppSettings["mailFrom"]));

            foreach (var item in ConfigurationManager.AppSettings["mailTo"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(new MailboxAddress(item));

            }

            foreach (var item in ConfigurationManager.AppSettings["mailCC"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.Cc.Add(new MailboxAddress(item));

            }

            message.Subject = "物料超期通知信息,请尽快处理!!!";

            string htmlBody = @"    <h3>Dear IQC Team:</h3>
    <h4>请登陆<a href=""http://10.130.44.18"">SMT-智能仓储系统</a> 超期信息，本邮件仅展示部分信息。</h4>
    <table style=""width:100%;"" border=""1"" cellspacing =""0"" cellpadding =""0"" >
        <tr>
            <th>料号</th>
            <th>料卷号</th>
            <th>D/C</th>
            <th>生产日期</th>
            <th>保质期(天)</th>
            <th>到期时间</th>
            <th>已延期(天)</th>
        </tr>";
            foreach (var item in reelOutLife)
            {
                htmlBody += string.Format(@"<tr>
            <td>{0}</td>
            <td>{1}</td>
            <td>{2}</td>
            <td>{3}</td>
            <td>{4}</td>
            <td>{5}</td>
            <td>{6}</td>
        </tr>", item.PartNoId, item.Id, item.DateCode, item.MakeDate.ToString("yyyy-MM-dd HH:mm:ss"), item.ShelfLife, item.MakeDate.AddDays(item.ShelfLife + item.ExtendShelfLife).ToString("yyyy-MM-dd HH:mm:ss"), item.ExtendShelfLife);
            }

            htmlBody += @"<tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlBody;
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, ec) => true;

                client.Connect(ConfigurationManager.AppSettings["mailHost"], int.Parse(ConfigurationManager.AppSettings["mailPort"]), false);

                // Note: only needed if the SMTP server requires authentication
                // client.Authenticate("joey", "password");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
