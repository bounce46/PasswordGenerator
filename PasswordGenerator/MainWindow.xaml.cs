using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int length = int.Parse(LengthTextBox.Text);
                if (length < 4 || length > 50)
                {
                    MessageBox.Show("Длина пароля должна быть от 4 до 50 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string chars = "";
                if (UppercaseCheckBox.IsChecked == true)
                    chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                if (LowercaseCheckBox.IsChecked == true)
                    chars += "abcdefghijklmnopqrstuvwxyz";
                if (NumbersCheckBox.IsChecked == true)
                    chars += "0123456789";
                if (SpecialCheckBox.IsChecked == true)
                    chars += "!@#$%^&*()_+-=[]{}|;:,.<>?";

                if (string.IsNullOrEmpty(chars))
                {
                    MessageBox.Show("Выберите хотя бы один тип символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                StringBuilder password = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    password.Append(chars[_random.Next(chars.Length)]);
                }

                PasswordTextBox.Text = password.ToString();

                // Анимация появления пароля
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                PasswordTextBox.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректное число для длины пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                Clipboard.SetText(PasswordTextBox.Text);
                MessageBox.Show("Пароль скопирован в буфер обмена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}