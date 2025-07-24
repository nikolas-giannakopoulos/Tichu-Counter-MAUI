using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices;


namespace Tichu_Counter;

public partial class DetailPage : ContentPage
{
    private int _formsWidth;
    private int _formsHeight;
    int counter = 1;
    private readonly MainPage mainPage;
    private List<HorizontalStackLayout> rounds = new List<HorizontalStackLayout>();
    private List<Line> lines = new List<Line>();
    public DetailPage(MainPage mainPage)
    {
        var _metrics = DeviceDisplay.MainDisplayInfo;
        this.mainPage = mainPage;
        _formsHeight = Convert.ToInt32(_metrics.Height / _metrics.Density);
        _formsWidth = Convert.ToInt32(_metrics.Width / _metrics.Density);
        InitializeComponent();
        view1.HeightRequest = _formsHeight * 0.8;
        FirstLine.X2 = _formsWidth * 0.8;
        DetailTeamA.WidthRequest = DetailTeamB.WidthRequest = _formsWidth * 0.20;
    }

    //Remove Last Score & Line
    public void Remove_LastRound()
    {
        scrollview.RemoveAt(scrollview.Count - 1);
        scrollview.RemoveAt(scrollview.Count - 1);
        rounds.RemoveAt(rounds.Count - 1);
    }

    //Remove all rounds and lines and readds first line
    public void Remove_AllRounds()
    {
        scrollview.Clear();
        rounds.Clear();
        lines.Clear();
        Line line = new Line()
        {
            X1 = 0,
            Y1 = 0,
            X2 = FirstLine.X2,
            Y2 = 0,
            Stroke = mainPage.getDarkTheme() ? Color.FromArgb("38464f") : Colors.LightGray,
            HorizontalOptions = LayoutOptions.Center
        };
        scrollview.Add(line);
        lines.Add(line);
    }
    
    //Creates a round score based on data from MainPage
    public void Register_Round(int teamA, int teamB)
    {
        HorizontalStackLayout views = new HorizontalStackLayout()
        {
            BackgroundColor = Colors.Transparent,
            Spacing = 30,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Opacity = 0
        };
        Color color_a ;
        Color color_b ;
        if (teamA > teamB)
        {
            color_a = Color.FromArgb("08cf6f");
            color_b = Color.FromArgb("fc4e69");
        }
        else if (teamA < teamB)
        {
            color_b = Color.FromArgb("08cf6f");
            color_a = Color.FromArgb("fc4e69");
        }
        else
        {
            color_a = color_b = Colors.Grey;
        }

        Label FirstTeam = new Label()
        {
            Text = teamA.ToString(),
            FontSize = 25,
            WidthRequest = DetailTeamA.WidthRequest,
            FontAttributes = FontAttributes.Bold,
            TextColor = color_a,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        Label SecondTeam = new Label()
        {
            Text = teamB.ToString(),
            FontSize = 25,
            WidthRequest = DetailTeamB.WidthRequest,
            FontAttributes = FontAttributes.Bold,
            TextColor = color_b,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center
        };

        Line line = new Line()
        {
            X1 = 0,
            Y1 = 0,
            X2 = FirstLine.X2,
            Y2 = 0,
            Stroke = mainPage.getDarkTheme() ? Color.FromArgb("38464f") : Colors.LightGray,
            HorizontalOptions = LayoutOptions.Center
        }; 
        views.Add(FirstTeam);
        views.Add(SecondTeam);
        scrollview.Add(views);
        scrollview.Add(line);
        rounds.Add(views);
        lines.Add(line);
    }

    public void ChangeLineTheme()
    {
        foreach(Line line in lines)
        {
            line.Stroke = mainPage.getDarkTheme() ? Color.FromArgb("38464f") : Colors.LightGray;
        }
    }

    //Updates LabelA when NamePopup accurs
    public void UpdateLabel_A(String TeamA)
    {
        String temp = TeamA;
        if (TeamA.Length > 7)
        {
            temp = TeamA.Substring(0, 3);
            temp += "/";
            temp += TeamA.Substring(TeamA.Length - 3, 3);
        }
        DetailTeamA.Text = temp;
    }

    //Updates LabelB when NamePopup accurs
    public void UpdateLabel_B(String TeamB)
    {
        String temp = TeamB;
        if(TeamB.Length > 7)
        {
            temp = TeamB.Substring(0, 3);
            temp += "/";
            temp += TeamB.Substring(TeamB.Length-3, 3);
        }
        DetailTeamB.Text = temp;
    }

    public async void RoundsAnimation()
    {
        foreach(HorizontalStackLayout round in rounds) { 
            await round.FadeTo(1, 100);
        }
    }

    //BackButton
    private async void BackButtonTheme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
        foreach (HorizontalStackLayout round in rounds)
        {
            round.Opacity = 0;
        }
    }
}