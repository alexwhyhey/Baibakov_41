using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baibakov_41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        int SignInFailedCount = 0;
        bool checkCaptcha = false;

        public AuthPage()
        {
            InitializeComponent();

            informationTextBlock.Text = "";
            captchaTextBox.Visibility = Visibility.Hidden;
            CaptchaPanel.Visibility = Visibility.Hidden;
        }

        private void SignInAsGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkCaptcha == true)
            {
                if (!check_captcha())
                {
                    failed_authorization();
                }
            }

            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;

            if (login == "" || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || password == "")
            {
                MessageBox.Show("Есть пустые поля или неверная капча");
                return;
            }

            User user = Baibakov_41Entities.GetContext().User.ToList().Find(x => x.UserLogin == login && x.UserPassword == password);
            if (user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                loginTextBox.Text = "";
                passwordTextBox.Text = "";
                SignInFailedCount = 0;
                checkCaptcha = false;
                CaptchaPanel.Visibility = Visibility.Hidden;
                captchaTextBox.Text = "";
                captchaTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Неверный пароль или такого пользователя не существует");
                checkCaptcha = true;
                failed_authorization();
            }
        }

        private void failed_authorization()
        {
            SignInFailedCount++;
            string symb = "abcdefghijklomnpqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();

            FirstWordCaptcha.Text = symb[rnd.Next(0, symb.Length)].ToString();
            SecondWordCaptcha.Text = symb[rnd.Next(0, symb.Length)].ToString();
            ThirdWordCaptcha.Text = symb[rnd.Next(0, symb.Length)].ToString();
            FourthWordCaptcha.Text = symb[rnd.Next(0, symb.Length)].ToString();
            CaptchaPanel.Visibility = Visibility.Visible;
            captchaTextBox.Visibility = Visibility.Visible;
            captchaTextBox.Text = "";

            if (SignInFailedCount > 1)
            {
                loginTextBox.Text = "";
                passwordTextBox.Text = "";
                wait_for(10);
            }
        }

        private async void wait_for(int secs)
        {
            SignInBtn.IsEnabled = false;

            for (int i = secs; i > 0; i--)
            {
                informationTextBlock.Text = $"Повторный вход через: {i} секунд";
                await Task.Delay(1000);
            }

            informationTextBlock.Text = "";
            SignInBtn.IsEnabled = true;
        }

        private bool check_captcha()
        {
            var captcha = FirstWordCaptcha.Text + SecondWordCaptcha.Text + ThirdWordCaptcha.Text + FourthWordCaptcha.Text;

            return captcha == captchaTextBox.Text;
        }
    }
}
