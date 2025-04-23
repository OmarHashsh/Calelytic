namespace Calelytic.UI
{
    public partial class MainPage : ContentPage
    {
        private DateTime today = DateTime.Today; // checks if today is today
        private DateTime currentDate = DateTime.Today;// tracks the currently visible month
        private Dictionary<string, Grid> loadedMonths = new(); // string = what month it is . Grid is a reference to the grid that's going to hold the month data
        private const int monthsToKeep = 12;
        private bool isAnimating = false;


        public MainPage()
        {
            InitializeComponent();
            InitializeCalendar();
            SetupEventListeners();
        }

        private void InitializeCalendar()
        {
            for (int i = -6; i <= 6; i++)
            {
                var date = currentDate.AddMonths(i);
                CreateMonth(date);
            }
            ShowCurrentMonth();
        }

        private void CreateMonth(DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startOffset = (int)firstDayOfMonth.DayOfWeek;

            // Start from Sunday (0) to Saturday (6)
            DateTime startDate = firstDayOfMonth.AddDays(-startOffset);

            foreach (HorizontalStackLayout ContainerChild in MonthsContainer)
            {
                foreach (Frame frame in ContainerChild)
                {
                    DateTime currentDay = startDate.AddDays(i);

                    if (frame == null) continue;

                    // Assume label is first child of the frame
                    if (frame.Content is Label label)
                    {
                        label.Text = currentDay.Day.ToString();

                        // Reset all styles
                        frame.StyleClass = new List<string> { "day" };

                        if (currentDay.Month != date.Month)
                            frame.StyleClass.Add("other-month");

                        if (currentDay.Date == today)
                            frame.StyleClass.Add("today");
                    }
                }

            }

                currentDate = date;
                CurrentMonthYear.Text = date.ToString("MMMM yyyy");
            }


        private void ShowCurrentMonth()
        {
            string key = GetMonthKey(currentDate);

            foreach (var month in loadedMonths.Values)
            {
                month.IsVisible = false;
            }

            if (loadedMonths.TryGetValue(key, out var currentMonth))
            {
                currentMonth.IsVisible = true;
                CurrentMonthYear.Text = currentDate.ToString("MMMM yyyy");
            }
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
            CreateMonth(newDate); // swaps label data
            await AnimateInLabels(-directionMultiplier);

            await Task.Delay(100); // slight pause after animation
            isAnimating = false;
        }

        private async Task AnimateOutLabels(int directionMultiplier)
        {
            var tasks = new List<Task>();

            foreach (var child in MonthsContainer.Children)
            {
                if (child is Frame frame && frame.Content is Label label)
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

            foreach (var child in MonthsContainer.Children)
            {
                if (child is Frame frame && frame.Content is Label label)
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
    }
}

