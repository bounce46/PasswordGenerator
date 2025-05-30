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
    /// 
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random();
        private readonly List<string> _passwordHistory = new List<string>();
        private readonly List<string> _wordList = new List<string>
        {
            "солнце", "луна", "звезда", "небо", "облако", "дерево", "река", "гора", "ветер", "цветок"
        };

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

                string password;
                if (MemorableCheckBox.IsChecked == true)
                {
                    password = GenerateMemorablePassword(length);
                }
                else
                {
                    password = GenerateRandomPassword(length);
                }

                PasswordTextBox.Text = password;
                UpdatePasswordStrength(password);
                AddToHistory(password);

                // Анимация появления пароля
                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                PasswordTextBox.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректное число для длины пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GenerateRandomPassword(int length)
        {
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
                return "";
            }

            StringBuilder password = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[_random.Next(chars.Length)]);
            }
            return password.ToString();
        }

        private string GenerateMemorablePassword(int length)
        {
            StringBuilder password = new StringBuilder();
            while (password.Length < length)
            {
                string word = _wordList[_random.Next(_wordList.Count)];
                if (UppercaseCheckBox.IsChecked == true)
                    word = char.ToUpper(word[0]) + word.Substring(1);
                password.Append(word);
                if (password.Length < length && (NumbersCheckBox.IsChecked == true || SpecialCheckBox.IsChecked == true))
                {
                    string chars = "";
                    if (NumbersCheckBox.IsChecked == true)
                        chars += "0123456789";
                    if (SpecialCheckBox.IsChecked == true)
                        chars += "!@#$";
                    if (!string.IsNullOrEmpty(chars))
                        password.Append(chars[_random.Next(chars.Length)]);

                }
            }
            return password.ToString().Substring(0, Math.Min(length, password.Length));
        }

        private void UpdatePasswordStrength(string password)
        {
            int score = 0;
            if (password.Length >= 8) score++;
            if (password.Any(char.IsUpper)) score++;
            if (password.Any(char.IsLower)) score++;
            if (password.Any(char.IsDigit)) score++;
            if (password.Any(ch => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(ch))) score++;

            StrengthIndicator.Width = score * 20;
            StrengthIndicator.Fill = score switch
            {
                <= 2 => Brushes.Red,
                <= 3 => Brushes.Orange,
                _ => Brushes.Green
            };
            StrengthText.Text = score switch
            {
                <= 2 => "Слабый",
                <= 3 => "Средний",
                _ => "Сильный"
            };
        }

        private void AddToHistory(string password)
        {
            _passwordHistory.Insert(0, password);
            if (_passwordHistory.Count > 5)
                _passwordHistory.RemoveAt(5);
            HistoryListBox.ItemsSource = null;
            HistoryListBox.ItemsSource = _passwordHistory;
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