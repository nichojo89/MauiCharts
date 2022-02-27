namespace MauiCharts.Charts;

public partial class PieChart : StackLayout
{
    public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(PieChart),
            new Dictionary<string, float>(),
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((PieChart)bindable);

                chartView.Chart.PieChartDrawable.Points = (Dictionary<string, float>)newValue;
            });

    public Dictionary<string, float> Points
    {
        get => (Dictionary<string, float>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public PieChart() => InitializeComponent();
}