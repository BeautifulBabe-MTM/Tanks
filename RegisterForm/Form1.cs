using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MimeKit;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Mail;

namespace RegisterForm
{
    public partial class Form1 : Form
    {
        public User user = new User();

        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 5 && textBox1.Text.Length < 12)
            {
                if (textBox2.Text.Length > 5 && textBox2.Text.Length < 12)
                {
                    if (!File.Exists(textBox1.Text + ".json"))
                    {
                        if (IsValidEmail(textBox3.Text))
                        {
                            user.login = textBox1.Text;
                            user.password = textBox2.Text;
                            user.mail = textBox3.Text;
                            SendPassword(user).GetAwaiter().GetResult();
                            File.WriteAllText($"{user.login}.json", JsonSerializer.Serialize<User>(user));
                        }
                    }
                }
            }
        }
        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static async Task SendPassword(User user)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress($"Wassup homie", "pitnichyk@gmail.com"));
            emailMessage.To.Add(new MailboxAddress($"Hello. How u been? {user.login}", user.mail));
            emailMessage.Subject = "Access info for game";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = ComputeSha256Hash(user.password)
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("pitnichyk@gmail.com", "s8k23d7d23");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        bool IsValidEmail(string mail)
        {
            if (mail.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(mail);
                return addr.Address == mail;
            }
            catch
            {
                return false;
            }
        }
    }
}