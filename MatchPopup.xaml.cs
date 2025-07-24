using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices;

namespace Tichu_Counter;

public partial class MatchPopup : Popup
{
    private readonly MainPage mainpage;
    private readonly HistoryPage historyPage;
    private String tempName;
    private bool accept_flag = false;
    private readonly bool EntryA;
    Label label;
    double heightt;
    int zeroMatchIndex;
    public MatchPopup(MainPage mainpage, int zeroMatchIndex)
    {
        this.mainpage = mainpage;
        this.zeroMatchIndex = zeroMatchIndex;
        InitializeComponent();
        popup.WidthRequest = Convert.ToInt32((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) * 0.7);
    }

    private void Cancel(object sender, EventArgs e)
    {
        this.CloseAsync();
    }

    private async void Accept(object sender, EventArgs e)
    {
        mainpage.getNameA().Text = "Team A";
        mainpage.getNameB().Text = "Team B";
        mainpage.getScoreA().Text = "0";
        mainpage.getScoreB().Text = "0";
        mainpage.getRoundsA().Clear();
        mainpage.getRoundsB().Clear();
        mainpage.getCurrentMatch().selected = false;
        mainpage.getCurrentMatch().box.IsEnabled = true;
        mainpage.getCurrentMatch().box.UpdateBGColor();
        //await mainpage.getDBManager().UpdateMatch(mainpage.getCurrentMatch());
        //mainpage.GetMatches()[zeroMatchIndex].selected = true;
        //mainpage.GetMatches()[zeroMatchIndex].box.IsEnabled = false;
        //mainpage.GetMatches()[zeroMatchIndex].box.UpdateBGColor();
        mainpage.selectedindex = zeroMatchIndex;
        mainpage.UpdateNames();
        //historypage.UpdateMatchIndex();
        mainpage.ResetButtons();

        this.CloseAsync();
    }

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
       
    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        //mainpage.getNewButton().IsEnabled = true;
    }
}