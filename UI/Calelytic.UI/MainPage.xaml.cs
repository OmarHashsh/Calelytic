namespace Calelytic.UI
{
    public partial class MainPage : ContentPage
    {
        private DateTime today = DateTime.Today; // checks if today is today
        private DateTime currentDate = DateTime.Today;// tracks the currently visible month
        private bool isAnimating = false;


        public MainPage()
        {
            InitializeComponent();
            InitializeCalendar();
            SetupEventListeners();
        }

        private void InitializeCalendar()
        {
            CreateMonth(currentDate);
        }

        private void CreateMonth(DateTime date)
        {
            // Clear existing days except weekday headers
            for (int i = MonthsContainer.Children.Count - 1; i >= 7; i--)
            {
                MonthsContainer.Children.RemoveAt(i);
            }

            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startOffset = (int)firstDayOfMonth.DayOfWeek;

            // Calculate total days to show (including previous and next month days)
            int totalDays = 42; // 6 weeks * 7 days
            DateTime startDate = firstDayOfMonth.AddDays(-startOffset);

            // Create day frames
            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDay = startDate.AddDays(i);
                int row = (i / 7) + 1; // +1 to account for weekday headers
                int col = i % 7;

                var border = new Border
                {
                    Style = (Style)Application.Current.Resources["dayBorder"]
                };

                var frame = new Frame
                {
                    Style = (Style)Application.Current.Resources["day"]
                };

                var label = new Label
                {
                    Style = (Style)Application.Current.Resources["day-number"],
                    Text = currentDay.Day.ToString()
                };

                // Set styles based on day type
                if (currentDay.Month != date.Month)
                {
                    border.StyleClass = new List<string> { "other-month" };
                    //frame.StyleClass = new List<string> { "other-month" };
                    label.StyleClass = new List<string> { "other-month-label" };
                }
                else if (currentDay.Date == today)
                {
                    border.StyleClass = new List<string> { "today" };
                    frame.StyleClass = new List<string> { "today" };
                    label.StyleClass = new List<string> { "today-label" };
                }

                frame.Content = label;
                border.Content = frame;
                MonthsContainer.Children.Add(border);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, col);
            }

            currentDate = date;
            CurrentMonthYear.Text = date.ToString("MMMM yyyy");
        }

        private void SetupEventListeners()
        {
            PrevMonth.Clicked += async (s, e) => await Navigate("prev");
            NextMonth.Clicked += async (s, e) => await Navigate("next");
        }

        private async Task Navigate(string direction)
        {
            if (isAnimating) return;
            isAnimating = true;

            var newDate = direction == "next" ? currentDate.AddMonths(1) : currentDate.AddMonths(-1);
            int directionMultiplier = direction == "next" ? -1 : 1;

            await AnimateOutLabels(directionMultiplier);
            CreateMonth(newDate);
            await AnimateInLabels(-directionMultiplier);

            await Task.Delay(100);
            isAnimating = false;
        }

        private async Task AnimateOutLabels(int directionMultiplier)
        {
            var tasks = new List<Task>();

            // Animate all day frames
            for (int i = 7; i < MonthsContainer.Children.Count; i++)
            {
                if (MonthsContainer.Children[i] is Frame frame && frame.Content is Label label)
                {
                    var translateTask = label.TranslateTo(100 * directionMultiplier, 0, 200, Easing.SinOut);
                    var fadeTask = label.FadeTo(0, 100, Easing.SinOut);
                    tasks.Add(Task.WhenAll(translateTask, fadeTask));
                }
            }

            tasks.Add(CurrentMonthYear.TranslateTo(100 * directionMultiplier, 0, 200, Easing.SinOut));
            tasks.Add(CurrentMonthYear.FadeTo(0, 200, Easing.SinOut));

            await Task.WhenAll(tasks);
        }

        private async Task AnimateInLabels(int directionMultiplier)
        {
            var tasks = new List<Task>();

            // Animate all day frames
            for (int i = 7; i < MonthsContainer.Children.Count; i++)
            {
                if (MonthsContainer.Children[i] is Frame frame && frame.Content is Label label)
                {
                    label.TranslationX = 100 * directionMultiplier;
                    label.Opacity = 0;
                    var translateTask = label.TranslateTo(0, 0, 200, Easing.SinIn);
                    var fadeTask = label.FadeTo(1, 200, Easing.SinIn);
                    tasks.Add(Task.WhenAll(translateTask, fadeTask));
                }
            }

            CurrentMonthYear.TranslationX = 100 * directionMultiplier;
            CurrentMonthYear.Opacity = 0;
            tasks.Add(CurrentMonthYear.TranslateTo(0, 0, 200, Easing.SinIn));
            tasks.Add(CurrentMonthYear.FadeTo(1, 200, Easing.SinIn));

            await Task.WhenAll(tasks);
        }

        private string GetMonthKey(DateTime date) => $"{date.Year}-{date.Month}";

        private void OnLayoutSizeChanged(object sender, EventArgs e)
        {
            if (sender is VerticalStackLayout layout)
            {
                // Get the parent ScrollView
                if (layout.Parent is ScrollView scrollView)
                {
                    // Set the minimum size to prevent shrinking
                    layout.MinimumHeightRequest = layout.Height;
                    layout.MinimumWidthRequest = layout.Width;
                }
            }
        }
    }
}

