using CommunityToolkit.Maui.Views;
namespace Tichu_Counter;

public partial class NameEditPopup : Popup
{
    private readonly MainPage mainPage;
    private readonly DetailPage detailPage;
    private readonly HistoryPage historyPage;
    private String defaultName;
    private bool accept_flag = false;
    private readonly bool EntryA;
    Label label;
    public NameEditPopup(MainPage mainPage, DetailPage detailPage, HistoryPage historyPage, Label label, bool EntryA)
    {
        InitializeComponent();
        this.mainPage = mainPage;
        this.detailPage = detailPage;
        this.historyPage = historyPage;
        this.label = label;
        this.EntryA = EntryA;
    }
    //DIEGRAPSE TO UPDATE FUNCTION GIATI DEN XREIAZETAI AFOU ALLAZEI DYNAMIKA APO XAML KAI VALE TO ENTRY VASISMENO SE AUTO TOU MAINPAGE

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        label.Text = NameEntry.Text;
    }

    private void Cancel_button_Clicked(object sender, EventArgs e)
    {
        this.CloseAsync();

    }

    private void Accept_button_Clicked(object sender, EventArgs e)
    {
        if (NameEntry.Text.Length < 1 || NameEntry.Text == new string(' ', NameEntry.Text.Length))
        {
            if (EntryA)
            {
                label.Text = NameEntry.Text = "Team A";
            }
            else
            {
                label.Text = NameEntry.Text = "Team B";
            }
        }

        if (EntryA)
        {
            detailPage.UpdateLabel_A(label.Text);
            //TODO: historyPage.UpdateNames
        }
        else
        {
            detailPage.UpdateLabel_B(label.Text);
            //TODO: historyPage.UpdateNames

        }
        accept_flag = true;
        mainPage.UpdateNames();
        this.CloseAsync();
    }

    private async void Pressed(object sender, EventArgs e)
    {
        await ((Button)sender).TranslateTo(0, 4, 20);
    }

    private async void Released(object sender, EventArgs e)
    {
        await ((Button)sender).TranslateTo(0, 0, 20);
    }
    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        NameEntry.Text = label.Text;
        defaultName = label.Text;
        NameEntry.Focus();
    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        if (!accept_flag)
        {
            label.Text = NameEntry.Text = defaultName;
        }
        mainPage.getNameA().IsEnabled = mainPage.getNameB().IsEnabled = true;
        mainPage.Focus();
    }

    public void UpdateBackgroundColor()
    {
        BackGround.BackgroundColor = mainPage.getDarkTheme() ? Color.FromArgb("191a1f") : Color.FromArgb("#f9f9f9");
        BackGround.Stroke = mainPage.getDarkTheme() ? Color.FromArgb("38464f") : Color.FromArgb("e5e5e5");
    }

}