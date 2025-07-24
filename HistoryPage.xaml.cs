using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Devices;

namespace Tichu_Counter
{

    public partial class HistoryPage : ContentPage
    {
        MainPage mainPage;
        private readonly DBManager dbManager = new DBManager();
        List<Match> matches = new List<Match>();
        List<Label> labels = new List<Label>();
        public bool check = false;
        int tett = 1;
        int zeroMatchesCount = 0;
        int _formsWidth;

        public HistoryPage(MainPage mainPage)
        {
            var _metrics = DeviceDisplay.MainDisplayInfo;
            var _formsHeight = Convert.ToInt32(_metrics.Height / _metrics.Density);
            _formsWidth = Convert.ToInt32(_metrics.Width / _metrics.Density);
            InitializeComponent();
            this.mainPage = mainPage;
            view1.HeightRequest = _formsHeight * 0.8;
            ScrollStack.WidthRequest = _formsWidth * 0.85;
            Create_Matches_FromDB();
        }

        //
        public async void MatchAnimation()
        {
            for (int i = matches.Count - 1; i >= 0; i--)
            {
                await matches[i].box.matchborder.FadeTo(1, 100);
                await Task.Delay(150);
            }
        }
        public async void LabelAnimation()
        {
            foreach (Label label in labels)
            {
                await label.FadeTo(1, 100);
            }
        }

        public void UpdateMatchBoxSequence(Match match) {
            try
            {

                ScrollStack.Remove(match.box);
                ScrollStack.Insert(1, match.box);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public VerticalStackLayout gg()
        {
            return VSL;
        }
        public StackLayout GetStackLayout()
        {
            return ScrollStack;
        }

        public MainPage GetMainPage()
        {
            return mainPage;
        }

        public void RequestMatchesDB()
        {
            try
            {
                matches.AddRange(dbManager.GetMatches());
            }
            catch
            {
                mainPage.ShowFailedConnectCard();
            }
        }

        public void Create_Matches_FromDB()
        {
            try
            {
                //Request DB Matches
                RequestMatchesDB();
                DateTime? past = null;

                if (matches.Count != 0)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        //Create MatchBox for every match from DB
                        matches[i].box = new MatchBox()
                        {
                            match = matches[i],
                            historypage = this
                        };
                        matches[i].box.Create();

                        //Select Current Match
                        if (matches[i].selected)
                        {
                            mainPage.setCurrentMatch(matches[i]);
                            mainPage.UpdateCurrentInfo();
                        }

                        //Add MatchBox on HistoryPage
                        AddMatchBox(matches[i].box, 0);
                        matches[i].isCreated = true;

                        if ((past != matches[i].date.Date || i == matches.Count - 1) && (i != 0 || matches.Count == 1))
                        {
                            DateTime now = DateTime.Now;
                            Label label;
                            String temp;

                            if (matches[i].date.Year == now.Year)
                            {
                                if (matches[i].date.Date == now.Date)
                                {
                                    temp = " Today";
                                }
                                else if (now.Date - matches[i].date.Date <= TimeSpan.FromDays(1))
                                {
                                    temp = " Yesterday";
                                }
                                else
                                {
                                    temp = matches[i].date.ToString(" d");
                                    switch (temp)
                                    {
                                        case " 1":
                                            temp += "st ";
                                            break;
                                        case " 2":
                                            temp += "nd ";
                                            break;
                                        case " 3":
                                            temp += "rd ";
                                            break;
                                        default:
                                            temp += "th ";
                                            break;
                                    }
                                    temp += matches[i].date.ToString("MMMM");
                                }

                                label = new Label()
                                {
                                    Text = temp,
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = mainPage.getDarkTheme() ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505"),
                                    FontSize = 16,
                                    Opacity = 0
                                };
                            }
                            else
                            {
                                label = new Label()
                                {
                                    Text = " " + matches[i].date.ToString("d"),
                                    TextColor = mainPage.getDarkTheme() ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505"),
                                };
                            }
                            AddLabel(label);
                        }
                        past = matches[i].date.Date;

                        if (matches[i].rounds_a.Count == 0)
                        {
                            zeroMatchesCount++;
                        }
                    }
                }
                else
                {
                    CreateMatch(false);
                    Label label = new Label()
                    {
                        Text = "Today",
                        TextColor = mainPage.getDarkTheme() ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505"),
                    };
                    AddLabel(label);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                mainPage.ShowFailedConnectCard();
            }
        }

        public async void CreateMatch(bool sameDate)
        {
            //Create Match Object
            Match match = new Match()
            {
                name_A = "Team A",
                name_B = "Team B",
                score_A = 0,
                score_B = 0,
                selected = true,
                rounds_a = new List<int>(),
                rounds_b = new List<int>(),
                date = DateTime.Now,
            };
            //Create MatchBox Object
            match.box = new MatchBox()
            {
                match = match,
                historypage = this
            };
            match.box.Create();
            matches.Add(match);

            if (sameDate)
            {
                AddMatchBox(match.box, 1);
            }
            else
            {
                AddMatchBox(match.box, 0);
            }
            match.selected = true;
            match.isCreated = true;
            if (matches.Count > 1)
            {
                mainPage.getCurrentMatch().selected = false;
                mainPage.getCurrentMatch().box.IsEnabled = true;
                mainPage.getCurrentMatch().box.UpdateBGColor();
                try
                {
                    await dbManager.UpdateMatch(mainPage.getCurrentMatch());
                }
                catch
                {
                    mainPage.ShowFailedConnectCard();
                }
            }

            mainPage.setCurrentMatch(match);

            try
            {
                await dbManager.AddMatch(match);
            }
            catch
            {
                mainPage.ShowFailedConnectCard();
            }
            zeroMatchesCount++;
        }

        public async void SelectMatch(object sender, TappedEventArgs e)
        {
            Border selectedborder = (Border)sender;
            if (!((MatchBox)selectedborder.BindingContext).match.selected)
            {
                //Change the visual of the box to showcase that it is selected
                selectedborder.Stroke = Color.FromArgb("6c63fe");
                selectedborder.BackgroundColor = mainPage.getDarkTheme() ? Color.FromArgb("252540") : Color.FromArgb("E0DFFE");

                //Change the value of selected and disable the SwipeView
                ((MatchBox)selectedborder.BindingContext).match.selected = true;
                ((MatchBox)selectedborder.BindingContext).match.box.IsEnabled = false;

                mainPage.getCurrentMatch().selected = false;
                mainPage.getCurrentMatch().box.IsEnabled = true;
                mainPage.getCurrentMatch().box.matchborder.BackgroundColor = mainPage.getDarkTheme() ? Color.FromArgb("1d1e24") : Color.FromArgb("FFFFFD");
                mainPage.getCurrentMatch().box.matchborder.Stroke = mainPage.getDarkTheme() ? Color.FromArgb("38464f") : Color.FromArgb("d6d6d6");
                try
                {
                    await dbManager.UpdateMatch(mainPage.getCurrentMatch());
                }
                catch {
                    mainPage.ShowFailedConnectCard();
                }

                mainPage.setCurrentMatch(((MatchBox)selectedborder.BindingContext).match);
                mainPage.UpdateCurrentInfo();
                try
                {
                    await dbManager.UpdateMatch(mainPage.getCurrentMatch());
                }
                catch
                {
                    mainPage.ShowFailedConnectCard();
                }
            }
        }

        public void DeleteSwipe(object sender, TappedEventArgs e)
        {
            var deletion = (SwipeView)((SwipeItems)((SwipeItemView)((Grid)((Image)sender).Parent).Parent).Parent).Parent;
            //ANIMATION
            DeleteMatch(deletion, matches.IndexOf(((MatchBox)((Image)sender).BindingContext).match));

        }

        public async void DeleteMatch(SwipeView swipeviewForDeletem, int indexForDelete)
        {
            RemoveLabel(matches[indexForDelete]);
            ScrollStack.Remove(swipeviewForDeletem);
            try
            {
                await dbManager.DeleteMatch(matches[indexForDelete]);
            }
            catch
            {
                mainPage.ShowFailedConnectCard();
            }
            matches.RemoveAt(indexForDelete);

            //In case of deleteing a 0-rounds match
            if (matches[indexForDelete].rounds_a.Count < 1)
            {
                SubstractZeroCounter();
            }
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            if (matches.Count > 0)
            {
                DeleteAll_Button.IsEnabled = false;
                var deleteall_Popup = new DeleteAllPopup(this);
                this.ShowPopup(deleteall_Popup);
            }
            else
            {
                //TODO: Show Card
            }
        }

        public async void DeleteAll()
        {
            DeleteAll_Button.IsVisible = false;
            try
            {
                dbManager.DeleteAll();
                await dbManager.AddMatch(mainPage.getCurrentMatch());
            }
            catch
            {
                mainPage.ShowFailedConnectCard();
            }

            for(int i = matches.Count - 1; i>=0; i--) {
                if (i < matches.Count - 1)
                {
                    await matches[i].box.TranslateTo(_formsWidth, 0, 150);
                }   
            }

            for (int i = ScrollStack.Children.Count - 1; i > 1; i--)
            {
                ScrollStack.Children.RemoveAt(i);
            }

            //TODO: Check εάν όλα δουλεύουν ορθά
            labels.Clear();
            labels.Add((Label)ScrollStack[0]);
            matches.Clear();
            matches.Add(mainPage.getCurrentMatch());
            
            zeroMatchesCount = (matches[0].rounds_a.Count > 0) ? 0 : 1;
        }
        public void RemoveLabel(Match match)
        {
            int index = GetStackLayout().IndexOf(match.box);
                if (GetStackLayout()[index - 1] is Label)
                {
                    try
                    {
                        if (GetStackLayout()[index + 1] is Label)
                        {
                            labels.Remove((Label)GetStackLayout()[index - 1]);
                            GetStackLayout().RemoveAt(index - 1);
                        }
                    }
                    catch
                    {
                        labels.Remove((Label)GetStackLayout()[index - 1]);
                        GetStackLayout().RemoveAt(index - 1);
                    }
                }
            
        }

        //Changes the Index of Match to bottom of List
        public async void UpdateMatchIndex()
        {
            try
            {
                //Removes the old position
                matches.Remove(mainPage.getCurrentMatch());

                //Adds Last in Queqe
                matches.Add(mainPage.getCurrentMatch());

                //Update sequence in HistoryPage List
                UpdateMatchBoxSequence(mainPage.getCurrentMatch());

                //Updates on DB
                await dbManager.UpdateMatch(mainPage.getCurrentMatch());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Whenever a matchcard is being swiped
        public void NewSwipe(object sender, SwipeStartedEventArgs e)
        {
            // Every swiped matchcard is being closed
            foreach (Match match in matches)
            {
                if (match.box != (SwipeView)sender)
                {
                    match.box.Close();
                }
            }
        }

        public void ChangeTheme()
        {
            foreach(Match match in matches)
            {
                match.box.UpdateBGColor();
            }
            foreach(Label label in labels)
            {
                label.TextColor = mainPage.getDarkTheme() ? Color.FromArgb("e5e5e5") : Color.FromArgb("050505");
            }
        }
        private async void BackButton(object sender, EventArgs e)
        {
            //Closing the page
            await Navigation.PopModalAsync();
            
            //Removing the Opacity of matches & labels for the animation to accur the following time the page is opened
            foreach (Match match in matches)
            {
                match.box.matchborder.Opacity = 0;
            }
            foreach(Label label in labels)
            {
                label.Opacity = 0;
            }

            DeleteAll_Button.IsVisible = true;
        }

        public bool IfMatchesExist()
        {
            return matches.Count > 0;
        }
        public void AddMatchBox(MatchBox matchbox, int index) {
            //Adding the match to the ScrollView
            ScrollStack.Insert(index, matchbox);
        }

        public void AddLabel(Label label)
        {
            //Adding the label to the top of the ScrollView
            ScrollStack.Insert(0, label);
            labels.Add(label);
        }

        public void SubstractZeroCounter()
        {
            zeroMatchesCount--;
        }

        public void AddZeroCounter()
        {
            zeroMatchesCount++;
        }

        public int getZeroCounter()
        {
            return zeroMatchesCount;
        }

        public void EnableDeleteAllButton()
        {
            DeleteAll_Button.IsEnabled = true;
        }
    }
}