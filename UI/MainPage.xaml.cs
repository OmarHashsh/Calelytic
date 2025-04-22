namespace UI
{
    public partial class CalendarPage : ContentPage
    {
        private DateTime today = DateTime.Today;
        private DateTime currentDate = DateTime.Today;
        private Dictionary<string, Grid> loadedMonths = new();
        private const int monthsToKeep = 5;
        private bool isAnimating = false;

        public CalendarPage()
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
            string monthKey = GetMonthKey(date);
            if (loadedMonths.ContainsKey(monthKey)) return;

            var monthGrid = new Grid
            {
                StyleClass = new[] { "month-grid" }
            };
            monthGrid.SetValue(VisualElement.AutomationIdProperty, monthKey);

            var label = new Label
            {
                Text = date.ToString("MMMM YYY"),
                StyleClass = new[] { "month-label" }
            };
            monthGrid.Children.Add(label);

            var calendarGrid = new Grid
            {
                StyleClass = new[] { "calendar-grid" }
            };

            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);
            var startDay = (int)firstDay.DayOfWeek;
            var totalDays = lastDay.Day;
            var prevMonthDays = firstDay.AddDays(-1).Day;

            // Previous month
            for (int i = startDay - 1; i >= 0; i--)
                calendarGrid.Children.Add(CreateDay(prevMonthDays - i, true));

            // Current month
            for (int i = 1; i <= totalDays; i++)
            {
                bool isToday = date.Month == today.Month && date.Year == today.Year && i == today.Day;
                calendarGrid.Children.Add(CreateDay(i, false, isToday));
            }

            // Next month filler
            int remaining = 42 - (startDay + totalDays);
            for (int i = 1; i <= remaining; i++)
                calendarGrid.Children.Add(CreateDay(i, true));

            monthGrid.Children.Add(calendarGrid);
            MonthsContainer.Children.Add(monthGrid);
            loadedMonths[monthKey] = monthGrid;
        }

        private View CreateDay(int number, bool isOtherMonth, bool isToday = false)
        {
            var frame = new Frame
            {
                Content = new Label
                {
                    Text = number.ToString(),
                    StyleClass = new[] { "day-number" }
                },
                StyleClass = new List<string> {
                "day",
                isOtherMonth ? "other-month" : "",
                isToday ? "today" : ""
            }.Where(c => !string.IsNullOrEmpty(c)).ToArray()
            };

            return frame;
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

            if (!loadedMonths.ContainsKey(GetMonthKey(newDate)))
            {
                //CreateMonth(newDate);
                CleanupOldMonths(direction);
            }

            currentDate = newDate;
            ShowCurrentMonth();

            await Task.Delay(300); // mimic animation delay
            isAnimating = false;
        }

        private void CleanupOldMonths(string direction)
        {
            var keys = loadedMonths.Keys.OrderBy(k => k).ToList();
            while (keys.Count > monthsToKeep)
            {
                var keyToRemove = direction == "next" ? keys[0] : keys[^1];
                if (loadedMonths.TryGetValue(keyToRemove, out var element))
                {
                    //MonthsContainer.Children.Remove(element);
                    loadedMonths.Remove(keyToRemove);
                }
                keys.Remove(keyToRemove);
            }
        }

        private string GetMonthKey(DateTime date) => $"{date.Year}-{date.Month}";
    }


}
