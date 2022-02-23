namespace MauiCharts.Charts;

public partial class LineChart : StackLayout
{
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(LineChart),
            new Dictionary<string, float>(),
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((LineChart)bindable);

                chartView.Chart.LineChartDrawable.Max = chartView.Points?.Select(x => x.Value).Max() * 1.1f ?? 0.0f;
                chartView.Chart.LineChartDrawable.Points = (Dictionary<string, float>)newValue;
            });

    public Dictionary<string, float> Points
    {
        get => (Dictionary<string, float>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }
    public LineChart()
	{
		InitializeComponent();
	}
}