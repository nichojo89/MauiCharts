namespace MauiCharts.Charts;

public partial class BarChart : StackLayout
{
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(BarChart),
            new Dictionary<string, float>(),
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((BarChart)bindable);

                //Give the heighest bar a little head room for aesthetics
                chartView.Chart.BarChartDrawable.Max = chartView.Points?.Select(x => x.Value).Max() * 1.1f ?? 0.0f;

                //Set the points from XAML to component
                chartView.Chart.BarChartDrawable.Points = (Dictionary<string, float>)newValue;
            });

    public Dictionary<string, float> Points
    {
        get => (Dictionary<string, float>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public BarChart() => InitializeComponent();
}