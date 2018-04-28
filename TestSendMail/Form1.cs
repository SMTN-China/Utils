using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Windows.Forms;
using System.Configuration;

namespace TestSendMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show(ConfigurationManager.AppSettings["mailTo"]);

    //        var message = new MimeMessage();
    //        message.From.Add(new MailboxAddress("SMT-智能仓储系统",ConfigurationManager.AppSettings["mailFrom"]));
    //        message.To.Add(new MailboxAddress(ConfigurationManager.AppSettings["mailTo"]));
    //        message.Cc.Add(new MailboxAddress(ConfigurationManager.AppSettings["mailCC"]));

    //        message.Subject = "物料超期通知信息,请尽快处理!!!";

            

    //        string htmlBody = @"    <h3>Dear IQC Team:</h3>
    //<h4>请登陆<a href=""http://10.130.44.18"">SMT-智能仓储系统</a> 超期信息</h4>
    //<table style=""width:100%;"" border=""1"" cellspacing =""0"" cellpadding =""0"" >
    //    <tr>
    //        <th>料号</th>
    //        <th>料卷号</th>
    //        <th>D/C</th>
    //        <th>生产日期</th>
    //        <th>保质期(天)</th>
    //        <th>到期时间</th>
    //        <th>已延期(天)</th>
    //    </tr>";
    //        //        foreach (var item in reelOutLife)
    //        //        {
    //        //            htmlBody += string.Format(@"<tr>
    //        //    <td>{0}</td>
    //        //    <td>{1}</td>
    //        //    <td>{2}</td>
    //        //    <td>{3}</td>
    //        //    <td>{4}</td>
    //        //    <td>{5}</td>
    //        //    <td>{6}</td>
    //        //</tr>", item.PartNoId, item.Id, item.DateCode, item.MakeDate.ToString("yyyy-MM-dd HH:mm:ss"), item.ShelfLife, item.MakeDate.AddDays(item.ShelfLife + item.ExtendShelfLife).ToString("yyyy-MM-dd HH:mm:ss"), item.ExtendShelfLife);
    //        //        }

    //        htmlBody += @"<tr>
    //                <td>&nbsp;</td>
    //                <td>&nbsp;</td>
    //                <td>&nbsp;</td>
    //                <td>&nbsp;</td>
    //                <td>&nbsp;</td>
    //                <td>&nbsp;</td>
    //                <td>&nbsp;</td>
    //            </tr>
    //        </table>";
    //        var bodyBuilder = new BodyBuilder();
    //        bodyBuilder.HtmlBody = htmlBody;
    //        message.Body = bodyBuilder.ToMessageBody();

    //        using (var client = new SmtpClient())
    //        {
    //            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
    //            client.ServerCertificateValidationCallback = (s, c, h, ec) => true;

    //            client.Connect(ConfigurationManager.AppSettings["mailHost"], int.Parse(ConfigurationManager.AppSettings["mailPort"]), false);
                
    //            // Note: only needed if the SMTP server requires authentication
    //            // client.Authenticate("joey", "password");

    //            client.Send(message);
    //            client.Disconnect(true);
    //        }



    //        MailMessage message = new MailMessage();

    //            message.From = new MailAddress(ConfigurationManager.AppSettings["mailFrom"], "SMT-智能仓储系统");

    //            message.To.Add(ConfigurationManager.AppSettings["mailTo"]);

    //            message.CC.Add(ConfigurationManager.AppSettings["mailCC"]);

    //            message.Subject = "物料超期通知信息,请尽快处理!!!";

    //            message.IsBodyHtml = true;

    //            string htmlBody = @"    <h3>Dear IQC Team:</h3>
    //<h4>请登陆<a href=""http://10.130.44.18"">SMT-智能仓储系统</a> 超期信息</h4>
    //<table style=""width:100%;"" border=""1"" cellspacing =""0"" cellpadding =""0"" >
    //    <tr>
    //        <th>料号</th>
    //        <th>料卷号</th>
    //        <th>D/C</th>
    //        <th>生产日期</th>
    //        <th>保质期(天)</th>
    //        <th>到期时间</th>
    //        <th>已延期(天)</th>
    //    </tr>";
    //    //        foreach (var item in reelOutLife)
    //    //        {
    //    //            htmlBody += string.Format(@"<tr>
    //    //    <td>{0}</td>
    //    //    <td>{1}</td>
    //    //    <td>{2}</td>
    //    //    <td>{3}</td>
    //    //    <td>{4}</td>
    //    //    <td>{5}</td>
    //    //    <td>{6}</td>
    //    //</tr>", item.PartNoId, item.Id, item.DateCode, item.MakeDate.ToString("yyyy-MM-dd HH:mm:ss"), item.ShelfLife, item.MakeDate.AddDays(item.ShelfLife + item.ExtendShelfLife).ToString("yyyy-MM-dd HH:mm:ss"), item.ExtendShelfLife);
    //    //        }

    //            htmlBody += @"<tr>
    //        <td>&nbsp;</td>
    //        <td>&nbsp;</td>
    //        <td>&nbsp;</td>
    //        <td>&nbsp;</td>
    //        <td>&nbsp;</td>
    //        <td>&nbsp;</td>
    //        <td>&nbsp;</td>
    //    </tr>
    //</table>";

    //            message.Body = htmlBody;

    //            using (var client = new SmtpClient(ConfigurationManager.AppSettings["mailHost"], int.Parse(ConfigurationManager.AppSettings["mailPort"])))
    //            {
    //                //// 主机
    //                //client.Host = ConfigurationManager.AppSettings["mailHost"];
    //                //// 端口
    //                //client.Port = int.Parse(ConfigurationManager.AppSettings["mailPort"]);

    //                // //设置发送人的邮箱账号和密码
    //                // client.Credentials = new NetworkCredential("", "");
    //                // //启用ssl,也就是安全发送
    //                // client.EnableSsl = true;

    //                client.Send(message);
    //            }
           
        }
    }
}
