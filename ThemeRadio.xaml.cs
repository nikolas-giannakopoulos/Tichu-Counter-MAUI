using CommunityToolkit.Maui.Views;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;
namespace Tichu_Counter;

public partial class ThemeRadio : Popup
{
    private readonly MainPage mainPage;
    public ThemeRadio(MainPage mainPage)
    {
        InitializeComponent();
        this.mainPage = mainPage;
        popup.WidthRequest = Convert.ToInt32((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) * 0.7);
        if(mainPage.getDarkTheme())
        {
            DarkThemeRadio.IsChecked = true;
        }
        else
        {
            LightThemeRadio.IsChecked = true;
        }
    }

    private void Cancel(object sender, EventArgs e)
    {
        this.CloseAsync();
    }

    private void Accept(object sender, EventArgs e)
    {
        this.CloseAsync();
    }

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {

    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
    }

    private void LightThemeRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (mainPage.getDarkTheme() != DarkThemeRadio.IsChecked)
        {
            mainPage.setDarkTheme(DarkThemeRadio.IsChecked);
            Preferences.Set("theme", DarkThemeRadio.IsChecked);
            Application.Current.UserAppTheme = DarkThemeRadio.IsChecked ? AppTheme.Dark : AppTheme.Light;
            mainPage.ChangeThemeButtons();
        }
    }
}