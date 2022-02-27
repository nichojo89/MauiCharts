namespace MauiCharts.Charts
{
    internal class LineChartGraphicsView : GraphicsView
    {
        public static readonly BindableProperty XAxisScaleProperty = BindableProperty.Create(nameof(XAxisScale),
        typeof(double),
        typeof(LineChartGraphicsView),
        0.0,
        propertyChanged: (b, o, n) => {
            var graphicsView = ((LineChartGraphicsView)b);
            graphicsView.LineChartDrawable.XAxisScale = Convert.ToSingle(n);
            graphicsView.Invalidate();
        });

        public double XAxisScale
        {
            get => (double)GetValue(XAxisScaleProperty);
            set => SetValue(XAxisScaleProperty, value);
        }

        public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(LineChartGraphicsView),
            new Dictionary<string, float>(),
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((LineChartGraphicsView)bindable);

                chartView.LineChartDrawable.Max = chartView.Points?.Select(x => x.Value).Max() * 1.1f ?? 0.0f;
                chartView.LineChartDrawable.Points = (Dictionary<string, float>)newValue;
            });

        public Dictionary<string, float> Points
        {
            get => (Dictionary<string, float>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public LineChartGraphicsView()
        {
            Drawable = LineChartDrawable;
        }

        public LineChartDrawable LineChartDrawable = new LineChartDrawable();
    }
}