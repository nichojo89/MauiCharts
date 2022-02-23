namespace MauiCharts.Charts
{
    internal class BarChartGraphicsView : GraphicsView
    {
        public static readonly BindableProperty XAxisScaleProperty = BindableProperty.Create(nameof(XAxisScale),
        typeof(double),
        typeof(BarChartGraphicsView),
        0.0,
        propertyChanged: (b, o, n) => {
            var graphicsView = ((BarChartGraphicsView)b);
            graphicsView.BarChartDrawable.XAxisScale = Convert.ToSingle(n);
            graphicsView.Invalidate();
        });

        public double XAxisScale
        {
            get => (double)GetValue(XAxisScaleProperty);
            set => SetValue(XAxisScaleProperty, value);
        }

        public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(BarChartGraphicsView),
            new Dictionary<string, float>(),
            propertyChanged: async (bindable, oldValue, newValue) =>
            {
                var chartView = ((BarChartGraphicsView)bindable);

                chartView.BarChartDrawable.Max = chartView.Points?.Select(x => x.Value).Max() * 1.1f ?? 0.0f;
                chartView.BarChartDrawable.Points = (Dictionary<string, float>)newValue;
            });

        public Dictionary<string, float> Points
        {
            get => (Dictionary<string, float>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public BarChartGraphicsView()
        {
            base.Drawable = BarChartDrawable;

            LoadChartAnimation();
        }

        /// <summary>
        /// Animates bars from 1/30 scale over 1 second
        /// </summary>
        public void LoadChartAnimation()
        {
            for (var i = 0; i <= 30; i++)
            {
                BarChartDrawable.BarScale = i / 30f;
                Invalidate();
                Task.Delay(33);
            }
            BarChartDrawable.ChartsLoading = false;
        }

        public BarChartDrawable BarChartDrawable = new BarChartDrawable();
    }
}
