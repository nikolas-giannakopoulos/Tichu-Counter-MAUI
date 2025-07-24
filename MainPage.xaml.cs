using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
namespace Tichu_Counter
{
    public partial class MainPage : ContentPage
    {
        int TichuA_flag, TichuB_flag, GrandA_flag, GrandB_flag, DoubleA_flag, DoubleB_flag =  0;
        int CurrentScore_A = 0;
        int CurrentScore_B = 0;
        bool Autoflag = false;
        bool first_undo = true;
        bool second_undo = false;
        bool second_undo_flag = false;
        public int selectedindex = 0;
        bool CardShown = false;
        public int temp_matches = 0;
        bool firstround = true;
        int LastScore_A;
        int LastScore_B;
        Match CurrentMatch = null;
        List<int> rounds_A = new List<int>();
        List<int> rounds_B = new List<int>();
        public string TextR
        {
            get { return "Giwrgos"; }
        }
        private readonly int _formsHeight;
        private readonly int _formsWidth;
        bool DarkTheme = false;
        HistoryPage historyPage;
        DetailPage detailPage;
        NameEditPopup nameEditPopup_A;
        NameEditPopup nameEditPopup_B;
        DBManager dbManager = new DBManager();
        SettingsBottomSheet settingsPage;

        public MainPage()
        {
            Application.Current.UserAppTheme = Preferences.Get("theme", false) ? AppTheme.Dark : AppTheme.Light;
            DarkTheme = Preferences.Get("theme", false);
            InitializeComponent();
            settingsPage = new SettingsBottomSheet(this);
            detailPage = new DetailPage(this);
            historyPage = new HistoryPage(this);
            var _metrics = DeviceDisplay.MainDisplayInfo;
            _formsHeight = Convert.ToInt32(_metrics.Height / _metrics.Density);
            _formsWidth = Convert.ToInt32(_metrics.Width / _metrics.Density);
            StartUp();

            //Check pote na thn kalei

            //
            //dbManager.DeleteT();
            //historyPage.Load_Matches();
            //selectedindex = 0;
            
            //NameA.Text = _formsHeight.ToString();
            NameB.Text = _formsWidth.ToString();
        }

        public void StartUp()
        {
            MenuStackLayout.Spacing = 0.5 * _formsWidth;
            settings_button.WidthRequest = settings_button.HeightRequest = (_formsHeight / _formsWidth) * 15.02;
            history_button.WidthRequest = history_button.HeightRequest = (_formsHeight / _formsWidth) * 15.02;
            details_button.WidthRequest = details_button.HeightRequest = (_formsHeight / _formsWidth) * 15.02;
            TeamA.HeightRequest = _formsHeight / 3.5;
            TeamB.HeightRequest = _formsHeight / 3.5;
            GamesVSL.Padding = new Thickness(0.069 * _formsWidth, 0, 0.069 * _formsWidth, 0);
            NameA.Margin = NameB.Margin = new Thickness(0.055 * _formsWidth, 0, 0, 0.005 * _formsWidth);
            ContainerA_VSL.Spacing = ContainerB_VSL.Spacing = 0.01 * _formsHeight;
            FrameA.WidthRequest = FrameB.WidthRequest = EntryA.WidthRequest = EntryB.WidthRequest = 0.17 * _formsWidth;
            FrameA.HeightRequest = FrameB.HeightRequest = 0.05 * _formsHeight;
            EntryA.HeightRequest = EntryB.HeightRequest = 0.077 * _formsHeight;
            ButtonsA_HSL.Spacing = ButtonsB_HSL.Spacing = 0.04 * _formsWidth;
            ButtonsA_HSL.Padding = ButtonsB_HSL.Padding = new Thickness(0.007 * _formsHeight, 0);
            LabelTichuA_VSL.Spacing = LabelGrandA_VSL.Spacing = LabelDoubleA_VSL.Spacing = LabelTichuΒ_VSL.Spacing = LabelGrandΒ_VSL.Spacing = LabelDoubleΒ_VSL.Spacing = 0.007 * _formsHeight;
            NotificationCard.TranslateTo(0, 100, 100);
        }

        private void UpdateVisual(object sender, Button behind, int button_state)
        {
            Button currentButton = (Button)sender;
            switch (button_state)
            {
                case 0:
                    currentButton.BackgroundColor = DarkTheme ? Color.FromArgb("1d1e24") : Color.FromArgb("FFFFFD");
                    currentButton.TextColor = DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505");
                    currentButton.BorderColor = DarkTheme ? Color.FromArgb("38464f") : Color.FromArgb("d6d6d6");
                    behind.BackgroundColor = DarkTheme ? Color.FromArgb("38464f") : Color.FromArgb("d6d6d6");
                    break;
                case 1:
                    currentButton.BackgroundColor = DarkTheme ? Color.FromArgb("1b302c") : Color.FromArgb("def3e4");
                    currentButton.TextColor = Color.FromArgb("08cf6f");
                    currentButton.BorderColor = Color.FromArgb("08cf6f");
                    behind.BackgroundColor = Color.FromArgb("08cf6f");
                    break;
                case 2:
                    currentButton.BackgroundColor = DarkTheme ? Color.FromArgb("34232b") : Color.FromArgb("ffe8e8");
                    currentButton.TextColor = Color.FromArgb("fc4e69");
                    currentButton.BorderColor = Color.FromArgb("fc4e69");
                    behind.BackgroundColor = Color.FromArgb("fc4e69");
                    break;

            }
        }

        private void UpdateLabel(int button_state, Label label, int number)
        {
            switch (button_state)
            {
                case 0:
                    label.TextColor = DarkTheme ? Color.FromArgb("1d1e24") : Color.FromArgb("FFFFFD");
                    break;
                case 1:
                    label.Text = "( +" + number.ToString() + " )";
                    label.TextColor = Color.FromArgb("08cf6f");
                    break;
                case 2:
                    label.Text = "( -" + number.ToString() + " )";
                    label.TextColor = Color.FromArgb("fc4e69");
                    break;
            }
        }

        private int CalculateExtras(int Tichu, int Grand, int DoubleWin)
        {
            if ((Tichu + Grand + DoubleWin) > 0)
            {
                int value = 0;

                if (Tichu == 1) { value += 100; }
                else if (Tichu == 2) { value -= 100; }

                if (Grand == 1) { value += 200; }
                else if (Grand == 2) { value -= 200; }

                if (DoubleWin == 1) { value += 200; }

                return value;
            }
            return 0;
        }

        private int UpdateState(int button_state, int TeamButton, int TeamDW, int EnemyButton1, int EnemyButton2, int EnemyDW)
        {
            switch (button_state)
            {
                case 0:
                    if (TeamButton == 1 || EnemyButton1 == 1 || EnemyButton2 == 1 || EnemyDW == 1)
                    {
                        return 2;
                    }
                    return 1;

                case 1:
                    if ((TeamButton == 2 && EnemyButton1 == 2 && EnemyButton2 == 2) || (TeamDW == 1 && TeamButton == 2))
                    {
                        return 0;
                    }
                    return 2;
            }
            return 0;
        }

        private int UpdateStateDW(int button_state, int TeamButton1, int TeamButton2, int EnemyButton1, int EnemyButton2, int EnemyDW)
        {
            return ((TeamButton1 == 2 && TeamButton2 == 2) || EnemyButton1 == 1 || EnemyButton2 == 1 || EnemyDW == 1 || button_state == 1) ? 0 : 1;
        }
        private async void TichuA_Clicked(object sender, EventArgs e)
        {
            TichuA_flag = UpdateState(TichuA_flag, GrandA_flag, DoubleA_flag, TichuB_flag, GrandB_flag, DoubleB_flag);
            Update_DoubleWin_State();

            UpdateLabel(TichuA_flag, TichuA_label, 100);
            UpdateVisual(sender, (Button)((Button)sender).BindingContext, TichuA_flag);
        }

        private async void Pressed(object sender, EventArgs e)
        {
            await ((Button)sender).TranslateTo(0, 4, 20);
        }

        private async void Released(object sender, EventArgs e)
        {
            await ((Button)sender).TranslateTo(0, 0, 20);
        }
        async private void Shake(VisualElement target)
        {
            uint timeout = 50;
            await target.TranslateTo(-8, 0, timeout);
            await target.TranslateTo(8, 0, timeout);
            await target.TranslateTo(-5, 0, timeout);
            await target.TranslateTo(5, 0, timeout);
            target.TranslationX = 0;
        }

        private async void GrandA_Clicked(object sender, EventArgs e)
        {
            GrandA_flag = UpdateState(GrandA_flag, TichuA_flag, DoubleA_flag, TichuB_flag, GrandB_flag, DoubleB_flag);
            Update_DoubleWin_State();
            UpdateLabel(GrandA_flag, GrandA_label, 200);
            UpdateVisual(sender, (Button)((Button)sender).BindingContext , GrandA_flag);
        }

        private void DoubleA_Clicked(object sender, EventArgs e)
        {
            DoubleA_flag = UpdateStateDW(DoubleA_flag, TichuA_flag, GrandA_flag, TichuB_flag, GrandB_flag, DoubleB_flag);
            Update_DoubleWin_State();
            UpdateLabel(DoubleA_flag, DoubleA_label, 200);
            UpdateVisual(sender, (Button)((Button)sender).BindingContext , DoubleA_flag);
            DisableEntries();
            if (TichuB_flag == 1)
            {
                Shake(TichuΒ_button);
                Shake(TichuB_label);
            }
            if (GrandB_flag == 1)
            {
                Shake(GrandΒ_button);
                Shake(GrandΒ_label);
            }
            if (DoubleB_flag == 1)
            {
                Shake(DoubleΒ_button);
                Shake(DoubleΒ_label);
            }
        }

        public async void ShowCard()
        {
            await NotificationCard.TranslateTo(0, -85, 150);
            await NotificationCard.TranslateTo(0, -65, 150);
        }

        public async void HideCard()
        {
            await NotificationCard.TranslateTo(0, -85, 150);
            await NotificationCard.TranslateTo(0, 155, 150);
        }
        private void TichuB_Clicked(object sender, EventArgs e)
        {
            TichuB_flag = UpdateState(TichuB_flag, GrandB_flag, DoubleB_flag, TichuA_flag, GrandA_flag, DoubleA_flag);
            Update_DoubleWin_State();
            UpdateLabel(TichuB_flag, TichuB_label, 100);
            UpdateVisual(sender, (Button)((Button)sender).BindingContext, TichuB_flag);

        }
        private void GrandB_Clicked(object sender, EventArgs e)
        {
            GrandB_flag = UpdateState(GrandB_flag, TichuB_flag, DoubleB_flag, TichuA_flag, GrandA_flag, DoubleA_flag);
            Update_DoubleWin_State();
            UpdateLabel(GrandB_flag, GrandΒ_label, 200);
            UpdateVisual(sender, (Button)((Button)sender).BindingContext, GrandB_flag);
           
        }
        private void DoubleB_Clicked(object sender, EventArgs e)
        {
            DoubleB_flag = UpdateStateDW(DoubleB_flag, TichuB_flag, GrandB_flag, TichuA_flag, GrandA_flag, DoubleA_flag);
            Update_DoubleWin_State();
            UpdateLabel(DoubleB_flag, DoubleΒ_label, 200);
            UpdateVisual(sender, (Button)((Button)sender).BindingContext, DoubleB_flag);
            DisableEntries();
            if (TichuA_flag == 1)
            {
                Shake(TichuA_button);
                Shake(TichuA_label);
            }
            if (GrandA_flag == 1)
            {
                Shake(GrandA_button);
                Shake(GrandA_label);
            }
            if (DoubleA_flag == 1)
            {
                Shake(DoubleA_button);
                Shake(DoubleA_label);
            }
        }

        public void Update_DoubleWin_State()
        {
            DoubleA_button.IsEnabled = !(TichuB_flag == 1 || GrandB_flag == 1 || DoubleB_flag == 1 || (TichuA_flag == 2 && GrandA_flag == 2));
            DoubleA_behind.IsEnabled = !(TichuB_flag == 1 || GrandB_flag == 1 || DoubleB_flag == 1 || (TichuA_flag == 2 && GrandA_flag == 2));
            DoubleA_button.TextColor = (TichuB_flag == 1 || GrandB_flag == 1 || DoubleB_flag == 1 || (TichuA_flag == 2 && GrandA_flag == 2)) ? (DarkTheme ? Color.FromArgb("434343") : Colors.Gray) : ReEnable_TextColor(DoubleA_flag);
            DoubleA_button.BackgroundColor = (TichuB_flag == 1 || GrandB_flag == 1 || DoubleB_flag == 1 || (TichuA_flag == 2 && GrandA_flag == 2)) ? (DarkTheme ? Color.FromArgb("6e6e6e") : Colors.LightGray) : ReEnable_Color(DoubleA_flag);
            DoubleA_behind.BackgroundColor = (TichuB_flag == 1 || GrandB_flag == 1 || DoubleB_flag == 1 || (TichuA_flag == 2 && GrandA_flag == 2)) ? (DarkTheme ? Color.FromArgb("434343") : Colors.Gray) : ReEnable_ColorBorder(DoubleA_flag);
            DoubleA_button.BorderColor = (TichuB_flag == 1 || GrandB_flag == 1 || DoubleB_flag == 1 || (TichuA_flag == 2 && GrandA_flag == 2)) ? (DarkTheme ? Color.FromArgb("434343") : Colors.Gray) : ReEnable_ColorBorder(DoubleA_flag);

            DoubleΒ_button.IsEnabled = !(TichuA_flag == 1 || GrandA_flag == 1 || DoubleA_flag == 1 || (TichuB_flag == 2 & GrandB_flag == 2));
            DoubleΒ_behind.IsEnabled = !(TichuA_flag == 1 || GrandA_flag == 1 || DoubleA_flag == 1 || (TichuB_flag == 2 & GrandB_flag == 2));
            DoubleΒ_button.TextColor = (TichuA_flag == 1 || GrandA_flag == 1 || DoubleA_flag == 1 || (TichuB_flag == 2 & GrandB_flag == 2)) ? (DarkTheme ? Color.FromArgb("434343") : Colors.Gray) : ReEnable_TextColor(DoubleB_flag);
            DoubleΒ_button.BackgroundColor = (TichuA_flag == 1 || GrandA_flag == 1 || DoubleA_flag == 1 || (TichuB_flag == 2 & GrandB_flag == 2)) ? (DarkTheme ? Color.FromArgb("6e6e6e") : Colors.LightGray) : ReEnable_Color(DoubleB_flag);
            DoubleΒ_behind.BackgroundColor = (TichuA_flag == 1 || GrandA_flag == 1 || DoubleA_flag == 1 || (TichuB_flag == 2 & GrandB_flag == 2)) ? (DarkTheme ? Color.FromArgb("434343") : Colors.Gray) : ReEnable_ColorBorder(DoubleB_flag);
            DoubleΒ_button.BorderColor = (TichuA_flag == 1 || GrandA_flag == 1 || DoubleA_flag == 1 || (TichuB_flag == 2 & GrandB_flag == 2)) ? (DarkTheme ? Color.FromArgb("434343") : Colors.Gray) : ReEnable_ColorBorder(DoubleB_flag);
        }

        private Color ReEnable_Color(int state)
        {
            if (state == 0)
            {
                return DarkTheme ? Color.FromArgb("1d1e24") : Color.FromArgb("FFFFFD");
            }
            return DarkTheme ? Color.FromArgb("1b302c") : Color.FromArgb("def3e4");
        }

        private Color ReEnable_ColorBorder(int state)
        {
            if (state == 0)
            {
                return DarkTheme ? Color.FromArgb("38464f") : Color.FromArgb("d6d6d6");
            }
            return Color.FromArgb("08cf6f");
        }

        private Color ReEnable_TextColor(int state)
        {
            if (state == 0)
            {
                return DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505");
            }
            return Color.FromArgb("08cf6f");
        }
       

        private void DisableEntries()
        {
            EntryA.Text = EntryB.Text = "";
            if ((DoubleA_flag + DoubleB_flag) == 0)
            {
                FrameA.ScaleTo(1, 150);
                FrameB.ScaleTo(1, 150);
                EntryA.IsEnabled = EntryB.IsEnabled = true;
            }
            else
            {
                FrameA.ScaleTo(0, 150);
                FrameB.ScaleTo(0, 150);
                EntryA.IsEnabled = EntryB.IsEnabled = false;
            }
        }

        public void ResetButtons()
        {
            TichuA_flag = 0;
            GrandA_flag = 0;
            DoubleA_flag = 0;
            TichuB_flag = 0;
            GrandB_flag = 0;
            DoubleB_flag = 0;
            UpdateVisual(TichuA_button, TichuA_behind, TichuA_flag);
            UpdateVisual(GrandA_button, GrandA_behind, GrandA_flag);
            UpdateVisual(TichuΒ_button, TichuΒ_behind, TichuB_flag);
            UpdateVisual(GrandΒ_button, GrandΒ_behind, GrandB_flag);
            Update_DoubleWin_State();

            UpdateLabel(TichuA_flag, TichuA_label, 100);
            UpdateLabel(GrandA_flag, GrandA_label, 200);
            UpdateLabel(DoubleA_flag, DoubleA_label, 200);
            UpdateLabel(TichuB_flag, TichuB_label, 100);
            UpdateLabel(GrandB_flag, GrandΒ_label, 200);
            UpdateLabel(DoubleB_flag, DoubleΒ_label, 200);

            FrameA.BackgroundColor = FrameB.BackgroundColor = DarkTheme ? Color.FromArgb("36373c") : Color.FromArgb("f2f2f9");
            //FTIAJTO LOGO 9.0
            //FrameA.BorderColor = FrameB.BorderColor = DarkTheme ? Color.FromArgb("191a1f") : Colors.White;
            EntryA.TextColor = EntryB.TextColor = DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505");
            DisableEntries();
        }

        public void ChangeThemeButtons()
        {
            UpdateVisual(TichuA_button, TichuA_behind, TichuA_flag);
            UpdateVisual(GrandA_button, GrandA_behind, GrandA_flag);
            UpdateVisual(TichuΒ_button, TichuΒ_behind, TichuB_flag);
            UpdateVisual(GrandΒ_button, GrandΒ_behind, GrandB_flag);
            Update_DoubleWin_State();

            UpdateLabel(TichuA_flag, TichuA_label, 100);
            UpdateLabel(GrandA_flag, GrandA_label, 200);
            UpdateLabel(DoubleA_flag, DoubleA_label, 200);
            UpdateLabel(TichuB_flag, TichuB_label, 100);
            UpdateLabel(GrandB_flag, GrandΒ_label, 200);
            UpdateLabel(DoubleB_flag, DoubleΒ_label, 200);

            FrameA.BackgroundColor = FrameB.BackgroundColor = DarkTheme ? Color.FromArgb("36373c") : Color.FromArgb("f2f2f9");
            //FTIAJTO LOGO 9.0
            //FrameA.BorderColor = FrameB.BorderColor = DarkTheme ? Color.FromArgb("191a1f") : Colors.White;
            EntryA.TextColor = EntryB.TextColor = DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505");

            historyPage.ChangeTheme();
            detailPage.ChangeLineTheme();
            //nameEditPopup_A.UpdateBackgroundColor();
            //nameEditPopup_B.UpdateBackgroundColor();
        }
        private bool isEmptyGame()
        {
            return rounds_A.Count < 1;
        }
        private void Undo(object sender, EventArgs e)
        {
            //VALE await UpdateLabelsAsync(1000); EDW GIA NA EXEI ANIMATION OTAN KANEI KAPOIOS UNDO

            //If there is an existing round
            if (!isEmptyGame())
            {
                try
                {
                    //Update Match Scores
                    CurrentMatch.UpdateMatch(0, 0, true);
                    historyPage.UpdateMatchIndex();
                    CurrentMatch.box.UpdateScores();
                    LastScore_A = rounds_A[rounds_A.Count - 1];
                    LastScore_B = rounds_B[rounds_B.Count - 1];
                    CurrentScore_A -= rounds_A[rounds_A.Count - 1];
                    CurrentScore_B -= rounds_B[rounds_B.Count - 1];

                    //Update Visual Score
                    ChangeScores();

                    //Update Current Rounds
                    rounds_A.RemoveAt(rounds_A.Count - 1);
                    rounds_B.RemoveAt(rounds_B.Count - 1);

                    //Remove Entry from DetailPages
                    detailPage.Remove_LastRound();

                    //Update Visuals
                    EntryA.Text = EntryB.Text = "";
                    ResetButtons();

                    if (isEmptyGame())
                    {
                        historyPage.AddZeroCounter();
                    }

                    //Redo the undo round
                    if (second_undo)
                    {
                        ShowCard();
                        CardShown = true;
                        Second_Undo_3s_timer();
                        //Minima katw katw gia ean thelei REDO
                    }
                    else
                    { 
                        second_undo = true;
                        Second_Undo_3s_timer();
                    }
                }
                catch
                {
                    ShowFailedSaveCard();
                }
            }
        }

        private async Task UpdateLabelsAsync(int duration)
        {
            SubmitButton.IsEnabled = false;
            SubmitBack.Opacity = 0;
            NewButton.IsEnabled = false;
            UndoButton.IsEnabled = false;

            int startA = CurrentScore_A - rounds_A[rounds_A.Count - 1];
            int startB = CurrentScore_B - rounds_B[rounds_B.Count - 1];
            int targetA = CurrentScore_A;
            int targetB = CurrentScore_B;
            int flag = 0;

            // Calculate the distance for both labels
            int distanceA = Math.Abs(rounds_A[rounds_A.Count - 1]);
            int distanceB = Math.Abs(rounds_B[rounds_B.Count - 1]);

            // Calculate the maximum distance
            int maxDistance = Math.Max(distanceA, distanceB);

            // Calculate the delay per step based on the maximum distance
            double delayPerStep = (double)duration / maxDistance;

            for (int step = 0; step <= maxDistance; step++)
            {
                // Calculate the current value for each label based on the step
                int currentA = startA + (int)(rounds_A[rounds_A.Count - 1] * ((double)step / maxDistance));
                int currentB = startB + (int)(rounds_B[rounds_B.Count - 1] * ((double)step / maxDistance));

                ScoreA.Text = currentA.ToString();
                ScoreB.Text = currentB.ToString();

                if (flag > maxDistance/70)
                {
                    await Task.Delay((int)delayPerStep);
                    flag = 0;
                }
                flag++;
            }

            // Ensure the final values are set
            ScoreA.Text = targetA.ToString();
            ScoreB.Text = targetB.ToString();

            SubmitButton.IsEnabled = true;
            SubmitBack.Opacity = 1;
            NewButton.IsEnabled = true;
            UndoButton.IsEnabled = true;
        }


        public async void ShowFailedSaveCard()
        {
            //Transform the card to error message
            RedoButton.IsVisible = false;
            CardText.HorizontalOptions = LayoutOptions.Center;
            CardText.HorizontalTextAlignment = TextAlignment.Center;
            CardText.Text = "There was an error. \nThis round WON'T be saved!";

            //Show the card for 3s until it goes off
            ShowCard();
            await Task.Delay(TimeSpan.FromSeconds(3));
            await NotificationCard.TranslateTo(0, -85, 150);
            await NotificationCard.TranslateTo(0, 155, 150);

            //Return to REDO stage card
            CardText.HorizontalOptions = LayoutOptions.Start;
            CardText.HorizontalTextAlignment = TextAlignment.Start;
            CardText.Text = "Went too fast?";
            RedoButton.IsVisible = true;
        }
        public async void ShowFailedConnectCard()
        {
            //Transform the card to error message
            RedoButton.IsVisible = false;
            CardText.HorizontalOptions = LayoutOptions.Center;
            CardText.HorizontalTextAlignment = TextAlignment.Center;
            CardText.Text = "There was an error. \nCAN'T load past matches!";

            //Show the card for 3s until it goes off
            ShowCard();
            await Task.Delay(TimeSpan.FromSeconds(3));
            await NotificationCard.TranslateTo(0, -85, 150);
            await NotificationCard.TranslateTo(0, 155, 150);

            //Return to REDO stage card
            CardText.HorizontalOptions = LayoutOptions.Start;
            CardText.HorizontalTextAlignment = TextAlignment.Start;
            CardText.Text = "Went too fast?";
            RedoButton.IsVisible = true;
        }
        public async void ShowAlreadyZeroMatchCard()
        {
            //Transform the card to error message
            RedoButton.IsVisible = false;
            CardText.HorizontalOptions = LayoutOptions.Center;
            CardText.HorizontalTextAlignment = TextAlignment.Center;
            CardText.Text = "Can't create new match.\nCurrent match is empty!";

            //Show the card for 3s until it goes off
            ShowCard();
            await Task.Delay(TimeSpan.FromSeconds(3));
            await NotificationCard.TranslateTo(0, -85, 150);
            await NotificationCard.TranslateTo(0, 155, 150);

            //Return to REDO stage card
            CardText.HorizontalOptions = LayoutOptions.Start;
            CardText.HorizontalTextAlignment = TextAlignment.Start;
            CardText.Text = "Went too fast?";
            RedoButton.IsVisible = true;
        }
        public async void ShowZeroMatchesCard()
        {
            //Transform the card to error message
            RedoButton.IsVisible = false;
            CardText.HorizontalOptions = LayoutOptions.Center;
            CardText.HorizontalTextAlignment = TextAlignment.Center;
            CardText.Text = "Can't create new match.\nFive 0-round matches already exist!";

            //Show the card for 3s until it goes off
            ShowCard();
            await Task.Delay(TimeSpan.FromSeconds(3));
            await NotificationCard.TranslateTo(0, -85, 150);
            await NotificationCard.TranslateTo(0, 155, 150);

            //Return to REDO stage card
            CardText.HorizontalOptions = LayoutOptions.Start;
            CardText.HorizontalTextAlignment = TextAlignment.Start;
            CardText.Text = "Went too fast?";
            RedoButton.IsVisible = true;
        }

        //5 Seconds for the chance of the Second Undo popup
        private async void Second_Undo_3s_timer()
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            if (CardShown)
            {
                CardShown = false;
                HideCard();
            }
            second_undo = false;
        }


        //Show Settings 
        private void Settings(object sender, EventArgs e)
        {
            settingsPage.ShowAsync(this.Window);
            //page.AnimationButtons();
        }

        //Show HistoryPage
        private async void History(object sender, EventArgs e) 
        {
            //ERROR 
            try
            {
                //display Match History Page
                await Navigation.PushModalAsync(historyPage);

                //Play matches and labels animation
                historyPage.MatchAnimation();
                historyPage.LabelAnimation();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
        }

        //Show DetailPage
        private async void Details(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(detailPage);
            detailPage.RoundsAnimation();
        }

        private async void Submit(object sender, EventArgs e)
        {
            try
            {
                if (isEmptyGame())
                {
                    historyPage.SubstractZeroCounter();
                }
                //Double Win 
                if (DoubleA_flag + DoubleB_flag > 0)
                {

                        //Calculate How many points From Extras
                        int ExtrasA = CalculateExtras(TichuA_flag, GrandA_flag, DoubleA_flag);
                        int ExtrasB = CalculateExtras(TichuB_flag, GrandB_flag, DoubleB_flag);

                        //Add Score & Round
                        CurrentScore_A += ExtrasA;
                        CurrentScore_B += ExtrasB;
                        rounds_A.Add(ExtrasA);
                        rounds_B.Add(ExtrasB);

                        //Apply Visual Changes

                        //UpdateLabelAsyncA();
                        //await UpdateLabelAsyncB();
                        //ChangeScores();
                        EntryA.Text = EntryB.Text = "";
                        ResetButtons();

                        //Updates Score Match
                        CurrentMatch.UpdateMatch(ExtrasA, ExtrasB, false);
                        //Change index of match to go last in queue & update DB
                        historyPage.UpdateMatchIndex();

                        //Add visual round on DetailPage
                        detailPage.Register_Round(ExtrasA, ExtrasB);
                        await UpdateLabelsAsync(1000);
                }
                //Not Double Win
                else if (Check(EntryA, EntryB))
                {
                    //Calculate How many points From Entries
                    int.TryParse(EntryA.Text, out int entry_a);
                    int.TryParse(EntryB.Text, out int entry_b);

                    //Calculate How many points From Extras
                    int ExtrasA = CalculateExtras(TichuA_flag, GrandA_flag, DoubleA_flag);
                    int ExtrasB = CalculateExtras(TichuB_flag, GrandB_flag, DoubleB_flag);

                    //Add Score & Round
                    CurrentScore_A += entry_a + ExtrasA;
                    CurrentScore_B += entry_b + ExtrasB;    
                    rounds_A.Add(entry_a + ExtrasA);
                    rounds_B.Add(entry_b + ExtrasB);


                    //Apply Visual Changes
                    //UpdateLabelAsyncA();
                    //await UpdateLabelAsyncB();
                    //ChangeScores();
                    EntryA.Text = "";
                    EntryB.Text = "";
                    ResetButtons();

                    //Updates Score Match
                    CurrentMatch.UpdateMatch(entry_a + ExtrasA, entry_b + ExtrasB, false);

                    //Add visual round on DetailPage
                    detailPage.Register_Round(rounds_A[rounds_A.Count - 1], rounds_B[rounds_B.Count - 1]);
                    await UpdateLabelsAsync(1000);
                }
            }
            catch
            {
                ShowFailedSaveCard();
            }
        }

        //Change Current Game List
        public void ChangeList(List<int> list_a, List<int> list_b)
        {
            rounds_A.Clear();
            rounds_B.Clear();
            rounds_A.AddRange(list_a);
            rounds_B.AddRange(list_b);
        }

        //Change Visual Scores
        public void ChangeScores()
        {
            ScoreA.Text = CurrentScore_A.ToString();
            ScoreB.Text = CurrentScore_B.ToString();
        }

        //New Match Button
        private void New(object sender, EventArgs e) 
        {
            //The match must not be empty
            if(!(rounds_A.Count == 0))
            {
                //Less than 5 zero-rounds match
                if(historyPage.getZeroCounter() < 5)
                {
                    ResetCurrentMatch();
                    detailPage.Remove_AllRounds();
                }
                else
                {
                    ShowZeroMatchesCard();
                }
            }
            else
            {
                ShowAlreadyZeroMatchCard();
            }
        }

        public async void ResetCurrentMatch()
        {
            rounds_A.Clear();
            rounds_B.Clear();
            NameA.Text = "Team A";
            NameB.Text = "Team B";
            ScoreA.Text = ScoreB.Text ="0";
            CurrentScore_A = CurrentScore_B = 0;
            ResetButtons();
            detailPage.Remove_AllRounds();

            if (historyPage.IfMatchesExist())
            {
                if (((MatchBox)historyPage.GetStackLayout()[1]).match.date.Date == DateTime.Now.Date)
                {
                    historyPage.CreateMatch(true);
                }
                else
                {
                    historyPage.CreateMatch(false);
                    Label label;
                    String temp = ((MatchBox)historyPage.GetStackLayout()[0]).match.date.ToString("d");


                    temp += ((MatchBox)historyPage.GetStackLayout()[0]).match.date.ToString("MMMM");
                    label = new Label()
                    {
                        Text = temp,
                        TextColor = DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505")
                    };
                    historyPage.AddLabel(label);
                }
            }
            else
            {
                historyPage.CreateMatch(false);
                Label label;
                String temp = ((MatchBox)historyPage.GetStackLayout()[0]).match.date.ToString("d");


                temp += ((MatchBox)historyPage.GetStackLayout()[0]).match.date.ToString("MMMM");
                label = new Label()
                {
                    Text = temp,
                    TextColor = DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505")
                };
                historyPage.AddLabel(label);
            }
        }

        /* public ImageButton getNewButton()
        {
            return NewButton;
        } */

        private void EntryA_complete(object sender, EventArgs e)
        {
            EntryA.Unfocus();
        }

        private void EntryA_update(object sender, EventArgs e)
        {
            Autoflag = !Autoflag;
            if (Autoflag)
            {
                AutoComplete(EntryA, EntryB);
            }
        }

        private void EntryB_complete(object sender, EventArgs e)
        {
            EntryB.Unfocus();
        }

        private void EntryB_update(object sender, EventArgs e)
        {
            Autoflag = !Autoflag;
            if (Autoflag)
            {
                AutoComplete(EntryB, EntryA);
            }
        }

        private void AutoComplete(Entry AllyEntry, Entry EnemyEntry)
        {
            // Checks the input of the User, and if it is a correct value, it automatically fills out the enemy score. If the value is wrong, the Entry turns Red and there is a shake animation. Also a value is returned, if the given input is in the correct format
            if (int.TryParse(AllyEntry.Text, out int input) && input < 126 && input > -26 && AllyEntry.Text != "" && AllyEntry.Text != "-" && AllyEntry.Text != "+")
            {
                if ((input > 12 || input < -2) && input % 5 != 0)
                {
                    FalseInput(AllyEntry);
                    (EnemyEntry).Text = "";
                }
                else if (input / 10 == 1 && input % 10 > 2 && input % 5 != 0)
                {
                    FalseInput(AllyEntry);
                    (EnemyEntry).Text = "";
                }
                else
                {
                    RightInput(AllyEntry);
                    RightInput(EnemyEntry);
                    EnemyEntry.Text = (100 - input).ToString();
                }
            }
            else if (AllyEntry.Text == "" || AllyEntry.Text == "-" || AllyEntry.Text == "+")
            {
                RightInput(AllyEntry);
                RightInput(EnemyEntry);
                EnemyEntry.Text = "";
            }
            else
            {
                FalseInput(AllyEntry);
                EnemyEntry.Text = "";
            }
        }

        private bool Check(Entry Entry_1, Entry Entry_2)
        {
            //Return the value of Entries
            int.TryParse(Entry_1.Text, out int input1);
            int.TryParse(Entry_2.Text, out int input2);
            
            //Check if [-25 - 125] & Not empty
            if ((input1 % 5 != 0 || input2 % 5 != 0 || input1 > 125 || input2 > 125 || input1 < -25 || input2 < -25) || (EntryA.Text == null || EntryA.Text == "" || EntryB.Text == null || EntryB.Text == ""))
            {
                FalseInput(Entry_1);
                FalseInput(Entry_2);
                return false;
            }
            return true;
        }


        private void FalseInput(Entry entry)
        {
            //If the input that was given was false, this method will be called and the Entry will "Shake" and the colors will be set to red.
            Shake((Frame)entry.Parent);
            ((Frame)entry.Parent).BackgroundColor = DarkTheme ? Color.FromArgb("5e3c45") : Color.FromArgb("ffe8e8");
            ((Frame)entry.Parent).BorderColor = Color.FromArgb("fc4e69");
            entry.TextColor = Color.FromArgb("fc4e69");
        }

        private void RightInput(Entry entry)
        {
            //If the input that was given was correct, this method will be called and the the colors of the Entry will be set to grey.
            ((Frame)entry.Parent).BackgroundColor = DarkTheme ? Color.FromArgb("36373c") : Color.FromArgb("f2f2f9");
            ((Frame)entry.Parent).BorderColor = DarkTheme ? Color.FromArgb("191a1f") : Colors.White;
            entry.TextColor = DarkTheme ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505");
    }

        public async void UpdateNames()
        {
            //Changes the Name of a Team
            CurrentMatch.name_A = NameA.Text;
            CurrentMatch.name_B = NameB.Text;
           // await dbManager.UpdateMatch(CurrentMatch);
            CurrentMatch.box.UpdateNames();
            detailPage.UpdateLabel_A(NameA.Text);
            detailPage.UpdateLabel_B(NameB.Text);
        }

        private void NameLabel_Tapped_A(object sender, TappedEventArgs e)
        {
            //Disable Name tap to not accur double Popup
            NameA.IsEnabled = NameB.IsEnabled = false;

            //Create and show PopUp
            try
            {
                nameEditPopup_A = new NameEditPopup(this, detailPage, historyPage, NameA, true);
                this.ShowPopup(nameEditPopup_A);
            }
            catch(Exception ex)
            {
                NameA.Text = ex.ToString();
            }
        }
        private void NameLabel_Tapped_B(object sender, TappedEventArgs e)
        {
            //Disable Name tap to not accur double Popup
            NameA.IsEnabled = NameB.IsEnabled = false;

            //Create and show PopUp
            nameEditPopup_B = new NameEditPopup(this, detailPage, historyPage, NameB, false);
            this.ShowPopup(nameEditPopup_B);
        }

        public void UpdateDetails()
        {
            try
            {
                detailPage.Remove_AllRounds();
                for(int i = 0; i < rounds_A.Count; i++)
                {
                    detailPage.Register_Round(rounds_A[i], rounds_B[i]);
                }
            }
            catch
            {
                ShowFailedSaveCard();
            }
        }

        public Label getNameA()
        {
            return NameA;
        }
        public Label getNameB()
        {
            return NameB;
        }

        public List<int> getRoundsA()
        {
            return rounds_A;
        }

        public List<int> getRoundsB()
        {
            return rounds_B;
        }

        public Label getScoreA()
        {
            return ScoreA;
        }

        public Label getScoreB()
        {
            return ScoreB;
        }

        public void setCurrentScore_A(int ScoreA)
        {
            CurrentScore_A = ScoreA;
        }

        public void setCurrentScore_B(int ScoreB)
        {
            CurrentScore_B = ScoreB;
        }



        private void Redo(object sender, TappedEventArgs e)
        {
            CardShown = false;
            HideCard();
            //Add Score & Round
            CurrentScore_A += LastScore_A;
            CurrentScore_B += LastScore_B;
            rounds_A.Add(LastScore_A);
            rounds_B.Add(LastScore_B);

            //Apply Visual Changes
            ChangeScores();
            EntryA.Text = EntryB.Text = "";
            ResetButtons();

            //Updates Score Match
            CurrentMatch.UpdateMatch(LastScore_A, LastScore_B, false);
            //Change index of match to go last in queue & update DB
            historyPage.UpdateMatchIndex();

            //Add visual round on DetailPage
            detailPage.Register_Round(LastScore_A, LastScore_B);
        }

        private async void ImageButton_Pressed(object sender, EventArgs e)
        {
            await SubmitButton.TranslateTo(0, 4, 20);

        }

        private async void ImageButton_Released(object sender, EventArgs e)
        {
            await SubmitButton.TranslateTo(0, 0, 20);
        }

        public Match getCurrentMatch()
        {
            return CurrentMatch;
        }

        public void setCurrentMatch(Match match)
        {
            CurrentMatch = match;
        }

        public void UpdateCurrentInfo()
        {
            try
            {
                CurrentScore_A = CurrentMatch.score_A;
                CurrentScore_B = CurrentMatch.score_B;
                ChangeList(CurrentMatch.rounds_a, CurrentMatch.rounds_b);
                UpdateDetails();
                //Visual Changes
                NameA.Text = CurrentMatch.name_A;
                NameB.Text = CurrentMatch.name_B;

                ScoreA.Text = CurrentScore_A.ToString();
                ScoreB.Text = CurrentScore_B.ToString();

                //Disable ScrollView of MatchBox
                CurrentMatch.box.IsEnabled = false;

                //Change Names on DetailPage
                detailPage.UpdateLabel_A(CurrentMatch.name_A);
                detailPage.UpdateLabel_B(CurrentMatch.name_B);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void setDarkTheme(bool darkTheme)
        {
            DarkTheme = darkTheme;
        }

        public bool getDarkTheme()
        {
            return DarkTheme;
        }
    }
}
