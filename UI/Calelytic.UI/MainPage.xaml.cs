using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Behaviors;
namespace Calelytic.UI
{
    public partial class MainPage : ContentPage
    {
        private DateTime today = DateTime.Today; // checks if the day we're examining is today
        private DateTime currentDate = DateTime.Today;// tracks the currently visible month
        private bool isAnimating = false;
        private Dictionary<ContentView, DateTime> frameDates = new Dictionary<ContentView, DateTime>();

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

                var contentView = new ContentView
                {
                    Style = (Style)Application.Current.Resources["day"]
                };

                // Store the date associated with this contentView
                frameDates[contentView] = currentDay;

                var dayNumberLabel = new Label
                {
                    Style = (Style)Application.Current.Resources["day-number"],
                    Text = currentDay.Day.ToString()
                };

                var eventLabel = new Label
                {
                    Style = (Style)Application.Current.Resources["event-label"],
                    Text = "Demo Body of text asjdkhasd"
                };

                var contentGrid = new Grid
                {
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Star }
                    }
                };

                contentGrid.Children.Add(dayNumberLabel);
                contentGrid.Children.Add(eventLabel);
                Grid.SetRow(eventLabel, 1);

                // Set styles based on day type
                if (currentDay.Month != date.Month)
                {
                    border.Style = (Style)Application.Current.Resources["other-month"];
                    dayNumberLabel.Style = (Style)Application.Current.Resources["other-month-label"];
                    eventLabel.Style = (Style)Application.Current.Resources["other-month-label"];
                }
                if (currentDay.Date == today.Date)
                {
                    border.Style = (Style)Application.Current.Resources["today"];
                    dayNumberLabel.Style = (Style)Application.Current.Resources["today-label"];
                    eventLabel.Style = (Style)Application.Current.Resources["event-label"];
                }

                contentView.Content = contentGrid;
                border.Content = contentView;
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

            // Add hover effect handlers
            foreach (var child in MonthsContainer.Children)
            {
                if (child is Border border && border.Content is ContentView contentView)
                {
                    // TODO: Replace the animation logic below with a popup logic on release. 
                    TapGestureRecognizer tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += (s, e) =>
                    {
                        if (frameDates.TryGetValue(contentView, out DateTime date))
                        {
                            contentView.Style = (Style)Application.Current.Resources["day-hover"];
                            // Reset style after a short delay
                            Task.Delay(200).ContinueWith(_ =>
                            {
                                MainThread.BeginInvokeOnMainThread(() =>
                                {
                                    contentView.Style = (Style)Application.Current.Resources["day"];
                                });
                            });
                        }
                    };

                    // Add pointer gesture for hover effect
                    var pointerGesture = new PointerGestureRecognizer();
                    pointerGesture.PointerEntered += (s, e) =>
                    {
                        if (frameDates.TryGetValue(contentView, out DateTime date))
                        {
                            // Apply hover style
                            contentView.Style = (Style)Application.Current.Resources["day-hover"];
                        }
                    };
                    pointerGesture.PointerExited += (s, e) =>
                    {
                        // Revert to normal style
                        contentView.Style = (Style)Application.Current.Resources["day"];
                    };

                    contentView.GestureRecognizers.Add(tapGesture);
                    contentView.GestureRecognizers.Add(pointerGesture);
                }
            }
        }
        

        private async Task Navigate(string direction)
        {
            if (isAnimating) return;
            isAnimating = true;

            var newDate = direction == "next" ? currentDate.AddMonths(1) : currentDate.AddMonths(-1);
            int directionMultiplier = direction == "next" ? -1 : 1;

            await AnimateOutLabels(directionMultiplier);
            CreateMonth(newDate);
            SetupEventListeners(); // Reattach event listeners to new month's elements
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
                if (MonthsContainer.Children[i] is Border border && border.Content is ContentView contentView)
                {
                    var translateTask = contentView.TranslateTo(100 * directionMultiplier, 0, 200, Easing.SinOut);
                    var fadeTask = contentView.FadeTo(0, 100, Easing.SinOut);
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
                if (MonthsContainer.Children[i] is Border border && border.Content is ContentView contentView)
                {
                    contentView.TranslationX = 100 * directionMultiplier;
                    contentView.Opacity = 0;
                    var translateTask = contentView.TranslateTo(0, 0, 200, Easing.SinIn);
                    var fadeTask = contentView.FadeTo(1, 200, Easing.SinIn);
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

