using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices;
namespace Tichu_Counter;

public partial class DeleteAllPopup : Popup
{
    private readonly HistoryPage historyPage;
    public DeleteAllPopup(HistoryPage historyPage)
    {
        this.historyPage = historyPage;
        InitializeComponent();
        popup.WidthRequest = Convert.ToInt32((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) * 0.7);
    }

    private void Cancel(object sender, EventArgs e)
    {
        this.CloseAsync();
    }

    private void Accept(object sender, EventArgs e)
    {
        historyPage.DeleteAll();
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
       
    }

    private void Popup_Closed(object sender, CommunityToolkit.Maui.Core.PopupClosedEventArgs e)
    {
        historyPage.EnableDeleteAllButton();
    }
}