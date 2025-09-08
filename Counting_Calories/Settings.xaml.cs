using Microsoft.Maui.Controls;

namespace Counting_Calories
{
    public partial class SettingsPage : ContentPage
    {
        private int themeIndex = 0; // 0 - Light, 1 - Dark, 2 - Colorful

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnThemeButtonClicked(object sender, EventArgs e)
        {
            themeIndex = (themeIndex + 1) % 3;

            switch (themeIndex)
            {
                case 0:
                    App.Current.UserAppTheme = AppTheme.Light; // ������� ����
                    App.ApplyLightTheme();
                    ThemeButton.Text = "����: �������";
                    break;

                case 1:
                    App.Current.UserAppTheme = AppTheme.Dark;  // ������ ����
                    App.ApplyDarkTheme();
                    ThemeButton.Text = "����: Ҹ����";
                    break;

                case 2:
                    App.Current.UserAppTheme = AppTheme.Unspecified; // ���������
                    App.ApplyColorfulTheme();
                    ThemeButton.Text = "����: �������";
                    break;
            }
        }


    }
}
