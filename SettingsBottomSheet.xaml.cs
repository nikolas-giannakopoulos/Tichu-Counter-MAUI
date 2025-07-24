using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Views;
using The49.Maui.BottomSheet;
using Microsoft.Maui.Devices;

namespace Tichu_Counter;

public partial class SettingsBottomSheet : BottomSheet
{
    private readonly int _formsWidth;
    private MainPage mainPage;
    private bool themeFlag = false; // False = dark || True = light
    private bool languageFLag = false; // False = English || True = Greek
    bool flag = false;
    public SettingsBottomSheet(MainPage mainPage)
    {
        var _metrics = DeviceDisplay.MainDisplayInfo;
        this.mainPage = mainPage;
        _formsWidth = Convert.ToInt32(_metrics.Width / _metrics.Density);
        InitializeComponent();
    }

    private void ThemeButton_Click(object sender, EventArgs e)
    {
        var themeradio = new ThemeRadio(mainPage);
        mainPage.ShowPopup(themeradio);
    }

    private void LightTheme_Click(object sender, EventArgs e)
    {
        if (!themeFlag)
        {
            LightTheme.BackgroundColor = Color.FromArgb("6c63fe");
            LightTheme.Stroke = Color.FromArgb("2B2598");
            LightTheme.StrokeThickness = 2;
            DarkTheme.BackgroundColor = Color.FromArgb("9D97F8");
            DarkTheme.StrokeThickness = 0;
            themeFlag = true;
        }
    }

    private void DarkTheme_Click(object sender, EventArgs e)
    {
        if (themeFlag)
        {
            DarkTheme.BackgroundColor = Color.FromArgb("6c63fe");
            DarkTheme.Stroke = Color.FromArgb("2B2598");
            DarkTheme.StrokeThickness = 2;
            LightTheme.BackgroundColor = Color.FromArgb("9D97F8");
            LightTheme.StrokeThickness = 0;
            themeFlag = false;
        }
    }

    private async void ChangeToLanguage()
    {
        English.TranslateTo(_formsWidth, 0, 1);
        Greek.TranslateTo(_formsWidth, 0, 1);
        BackButtonLanguage.TranslateTo(_formsWidth, 0, 1);
        ThemeButton.TranslateTo(-_formsWidth, 0, 180);
        LanguageButton.TranslateTo(-_formsWidth, 0, 180);
        HelpButton.TranslateTo(-_formsWidth, 0, 180);
        AboutButton.TranslateTo(-_formsWidth, 0, 180);
        await BackButton.TranslateTo(-_formsWidth, 0, 180);
        ThemeButton.IsVisible = false;
        LanguageButton.IsVisible = false;
        HelpButton.IsVisible = false;
        AboutButton.IsVisible = false;
        BackButton.IsVisible = false;
        English.IsVisible = true;
        Greek.IsVisible = true;
        BackButtonLanguage.IsVisible = true;
        English.TranslateTo(0, 0, 180);
        Greek.TranslateTo(0, 0, 180);
        await BackButtonLanguage.TranslateTo(0, 0, 180);
    }
    private async void ChangeToTheme()
    {
        LightTheme.TranslateTo(_formsWidth, 0, 1);
        DarkTheme.TranslateTo(_formsWidth, 0, 1);
        BackButtonTheme.TranslateTo(_formsWidth, 0, 1);
        ThemeButton.TranslateTo(-_formsWidth, 0, 180);
        LanguageButton.TranslateTo(-_formsWidth, 0, 180);
        HelpButton.TranslateTo(-_formsWidth, 0, 180);
        AboutButton.TranslateTo(-_formsWidth, 0, 180);
        await BackButton.TranslateTo(-_formsWidth, 0, 180);
        ThemeButton.IsVisible = false;
        LanguageButton.IsVisible = false;
        HelpButton.IsVisible = false;
        AboutButton.IsVisible = false;
        BackButton.IsVisible = false;
        LightTheme.IsVisible = true;
        DarkTheme.IsVisible = true;
        BackButtonTheme.IsVisible = true;
        LightTheme.TranslateTo(0, 0, 180);
        DarkTheme.TranslateTo(0, 0, 180);
        await BackButtonTheme.TranslateTo(0, 0, 180);
    }

    private async void BackButtonTheme_Click(object sender, EventArgs e)
    {
        LightTheme.TranslateTo(_formsWidth, 0, 180);
        DarkTheme.TranslateTo(_formsWidth, 0, 180);
        await BackButtonTheme.TranslateTo(_formsWidth, 0, 180);
        LightTheme.IsVisible = false;
        DarkTheme.IsVisible = false;
        BackButtonTheme.IsVisible = false;
        ThemeButton.IsVisible = true;
        LanguageButton.IsVisible = true;
        HelpButton.IsVisible = true;
        AboutButton.IsVisible = true;
        BackButton.IsVisible = true;
        ThemeButton.TranslateTo(0, 0, 180);
        LanguageButton.TranslateTo(0, 0, 180);
        HelpButton.TranslateTo(0, 0, 180);
        AboutButton.TranslateTo(0, 0, 180);
        await BackButton.TranslateTo(0, 0, 180);
    }
    private async void BackButtonLanguage_Click(object sender, EventArgs e)
    {
        English.TranslateTo(_formsWidth, 0, 180);
        Greek.TranslateTo(_formsWidth, 0, 180);
        await BackButtonLanguage.TranslateTo(_formsWidth, 0, 250);
        English.IsVisible = false;
        Greek.IsVisible = false;
        BackButtonLanguage.IsVisible = false;
        ThemeButton.IsVisible = true;
        LanguageButton.IsVisible = true;
        HelpButton.IsVisible = true;
        AboutButton.IsVisible = true;
        BackButton.IsVisible = true;
        ThemeButton.TranslateTo(0, 0, 180);
        LanguageButton.TranslateTo(0, 0, 180);
        HelpButton.TranslateTo(0, 0, 180);
        AboutButton.TranslateTo(0, 0, 180);
        await BackButton.TranslateTo(0, 0, 180);
    }
    private void English_Click(object sender, EventArgs e)
    {
        if (!languageFLag)
        {
            English.BackgroundColor = Color.FromArgb("6c63fe");
            English.Stroke = Color.FromArgb("2B2598");
            English.StrokeThickness = 2;
            Greek.BackgroundColor = Color.FromArgb("9D97F8");
            Greek.StrokeThickness = 0;
            languageFLag = true;
        }
    }

    private void BottomSheet_Dismissed(object sender, DismissOrigin e)
    {
        LightTheme.TranslateTo(_formsWidth, 0, 1);
        DarkTheme.TranslateTo(_formsWidth, 0, 1);
        BackButtonTheme.TranslateTo(_formsWidth, 0, 1);
        LightTheme.IsVisible = false;
        DarkTheme.IsVisible = false;
        BackButtonTheme.IsVisible = false;
        English.TranslateTo(_formsWidth, 0, 1);
        Greek.TranslateTo(_formsWidth, 0, 1);
        BackButtonLanguage.TranslateTo(_formsWidth, 0, 1);
        English.IsVisible = false;
        Greek.IsVisible = false;
        BackButtonLanguage.IsVisible = false;
        ThemeButton.IsVisible = true;
        LanguageButton.IsVisible = true;
        HelpButton.IsVisible = true;
        AboutButton.IsVisible = true;
        BackButton.IsVisible = true;
        ThemeButton.TranslateTo(0, 0, 1);
        LanguageButton.TranslateTo(0, 0, 1);
        HelpButton.TranslateTo(0, 0, 1);
        AboutButton.TranslateTo(0, 0, 1);
        BackButton.TranslateTo(0, 0, 1);
    }

    private void Greek_Click(object sender, EventArgs e)
    {
        if (languageFLag)
        {
            Greek.BackgroundColor = Color.FromArgb("6c63fe");
            Greek.Stroke = Color.FromArgb("2B2598");
            Greek.StrokeThickness = 2;
            English.BackgroundColor = Color.FromArgb("9D97F8");
            English.StrokeThickness = 0;
            languageFLag = false;
        }
    }
    private void LanguageButton_Click(object sender, EventArgs args)
    {
        ChangeToLanguage();
    }
    private void HelpButton_Click(object sender, EventArgs args)
    {
    }


    private void AboutButton_Click(object sender, EventArgs args)
    {
        if (flag)
        {
            AboutButton.BackgroundColor = Colors.Yellow;
        }
        else
        {
            AboutButton.BackgroundColor = Colors.Red;

        }
        flag = !flag;
    }
    /*async private void FadeIn(VisualElement target)
    {
        uint timeout = 250;
        await target.FadeTo(0.25, timeout);
        await target.FadeTo(0.5, timeout);
        await target.FadeTo(0.75, timeout);
        await target.FadeTo(1, timeout);
    }*/

    private void BackButton_Click(object sender, EventArgs args)
    {
        this.DismissAsync();
    }
}