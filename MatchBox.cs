using Microsoft.Maui.Controls.Shapes;

namespace Tichu_Counter
{
    public class MatchBox : SwipeView
    {
        public TapGestureRecognizer tapGestureRecognizer;
        public Border matchborder;
        private readonly int _formsWidth;
        Label NameA;
        Label NameB;
        Label ScoreA;
        Label ScoreB;
        Label Half;
        Label Half2;
        Label Date;
        public HistoryPage historypage {  get; set; }
        public Match match { get; set; }
        public MatchBox()
        {
            Padding = new Thickness(0, 0, 0, 10);
        }

        public void Create()
        {
                this.SwipeStarted += historypage.NewSwipe;
                Image deleteImage = new Image()
                {
                    Source = "bin_icon.png",
                    WidthRequest = 40,
                    HeightRequest = 40,
                };
                tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += historypage.DeleteSwipe;
                deleteImage.BindingContext = this;
                deleteImage.GestureRecognizers.Add(tapGestureRecognizer);

                Grid grid = new Grid()
            {
                deleteImage
            };
                grid.WidthRequest = 80;
                grid.HeightRequest = 100;

                SwipeItemView nik = new SwipeItemView();
                nik.Content = grid;
                this.RightItems = new SwipeItems() { nik };
                this.Content = CreateBorder(historypage.GetStackLayout());
        }

        public Border CreateBorder(StackLayout _stackLayout)
        {
            HorizontalStackLayout horizontalView = new HorizontalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center, 
                Spacing = 5
            };
            VerticalStackLayout teamA = new VerticalStackLayout()
            {
                WidthRequest = ((Convert.ToInt32(_stackLayout.WidthRequest) - 10) / 2) - 20
            };
            VerticalStackLayout teamB = new VerticalStackLayout()
            {
                WidthRequest = ((Convert.ToInt32(_stackLayout.WidthRequest) - 10) / 2) - 20
            };
            VerticalStackLayout halfer = new VerticalStackLayout() { WidthRequest = 30 };
            NameA = new Label()
            {
                Text = match.name_A,
                FontSize = 18,
                TextColor = Color.FromArgb("6c63fe"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                MaxLines = 1,
                LineBreakMode = LineBreakMode.TailTruncation
            };
            ScoreA = new Label()
            {
                Text = match.score_A.ToString(),
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
            };
            NameB = new Label()
            {
                Text = match.name_B,
                FontSize = 18,
                TextColor = Color.FromArgb("6c63fe"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                MaxLines = 1,
                LineBreakMode = LineBreakMode.TailTruncation
            };
            ScoreB = new Label()
            {
                Text = match.score_B.ToString(),
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            Half = new Label()
            {
                Text = "VS",
                TextColor = Color.FromArgb("6c63fe"),
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            /*Half2 = new Label()
            {
                Text = ":",
                FontSize = 35,
                TextColor = Color.FromArgb("6c63fe"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };*/
            Date = new Label()
            {
                Text = match.date.ToString(),
                FontSize = 15,
                HorizontalOptions = LayoutOptions.End
            };
            if (match.score_A > match.score_B)
            {
                ScoreA.TextColor = Color.FromArgb("08cf6f");
                ScoreB.TextColor = Color.FromArgb("fc4e69");
            }
            else if (match.score_A < match.score_B)
            {
                ScoreA.TextColor = Color.FromArgb("fc4e69");
                ScoreB.TextColor = Color.FromArgb("08cf6f");
            }
            else
            {
                ScoreA.TextColor = ScoreB.TextColor = Colors.Grey;
            }
            teamA.Add(NameA);
            teamA.Add(ScoreA);
            teamB.Add(NameB);
            teamB.Add(ScoreB);
            halfer.Add(Half);
            halfer.Add(Half2);
            horizontalView.Add(teamA);
            horizontalView.Add(halfer);
            horizontalView.Add(teamB);
            
            matchborder = new Border()
            {
                BackgroundColor = historypage.GetMainPage().getDarkTheme() ? Color.FromArgb("1d1e24") : Color.FromArgb("FFFFFD"),
                WidthRequest = Convert.ToInt32(_stackLayout.WidthRequest),
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(5),
                StrokeThickness = 2,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                Content = horizontalView,
                Stroke = historypage.GetMainPage().getDarkTheme() ? Color.FromArgb("38464f") : Color.FromArgb("d6d6d6"),
                Opacity = 0,
                
                //  TYPOU BUTTON SHADDOW
                //Shadow = new Shadow { Brush = Color.FromArgb("#E5E5E5"), Offset= new Point(0,3), Radius= 0 }
            };
            if (match.selected)
            {
                matchborder.Stroke = Color.FromArgb("6c63fe");
                matchborder.BackgroundColor = historypage.GetMainPage().getDarkTheme() ? Color.FromArgb("252540") : Color.FromArgb("E0DFFE");
                this.IsEnabled = false;
                //NameA.TextColor = Colors.Black;
               //// NameB.TextColor = Colors.Black;
                //Half.TextColor = Colors.Black;
                
            }
            TapGestureRecognizer tapBorder = new TapGestureRecognizer();
            tapBorder.Tapped += historypage.SelectMatch;
            matchborder.GestureRecognizers.Add(tapBorder);
            matchborder.BindingContext = this;
            return matchborder;
        }
        public void UpdateScores()
        {
            ScoreA.Text = match.score_A.ToString();
            ScoreB.Text = match.score_B.ToString();
            if (match.score_A > match.score_B)
            {
                ScoreA.TextColor = Color.FromArgb("08cf6f");
                ScoreB.TextColor = Color.FromArgb("fc4e69");
            }
            else if (match.score_A < match.score_B)
            {
                ScoreA.TextColor = Color.FromArgb("fc4e69");
                ScoreB.TextColor = Color.FromArgb("08cf6f");
            }
            else
            {
                ScoreA.TextColor = ScoreB.TextColor = Colors.Grey;
            }
            historypage.GetStackLayout().Remove(this);
            historypage.GetStackLayout().Insert(1, this);
        }

        public void UpdateNames()
        {
            NameA.Text = match.name_A;
            NameB.Text = match.name_B;
        }
        public void UpdateBGColor()
        {
            if (match.selected)
            {
                matchborder.Stroke = Color.FromArgb("6c63fe");
                matchborder.BackgroundColor = historypage.GetMainPage().getDarkTheme() ? Color.FromArgb("252540") : Color.FromArgb("E0DFFE");
            }
            else
            {
                matchborder.BackgroundColor = historypage.GetMainPage().getDarkTheme() ? Color.FromArgb("1d1e24") : Color.FromArgb("FFFFFD");
                matchborder.Stroke = historypage.GetMainPage().getDarkTheme() ? Color.FromArgb("38464f") : Color.FromArgb("d6d6d6");
            }
        }
    }
}