namespace MauiCharts.Charts
{
    internal class BarChartDrawable : View, IDrawable
    {
        public Dictionary<string, float> Points
        {
            get => _points;
            set
            { 
                _points = value;
                OnPropertyChanged();
            }
        }

        public double XAxisScale
        {
            get => _xAxisScale;
            set { 
            _xAxisScale = value;
                OnPropertyChanged();
            }
        }

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            const int BAR_WIDTH = 20;

            canvas.FontColor = Color.FromArgb("#7F2CF6");

            _chartWidth = dirtyRect.Width;

            //If the slider was moved then change x axis for the first bar
            if (XAxisScale != XAxisScaleOrigin)
                _firstBarXAxis += (float)(XAxisScale - XAxisScaleOrigin) * _chartWidth * -1;

            var barXAxis = _firstBarXAxis;
            //passing RGB ints to constructor does not work, should I submit a PR??
            var transparentMauiPurpleColor = Color.FromRgba(178, 127, 255, 0);
            var mauiPurpleColor = Color.FromRgb(178, 127, 255);

            var linearGradientPaint = new LinearGradientPaint
            {
                StartColor = mauiPurpleColor,
                EndColor = transparentMauiPurpleColor,
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

            canvas.SetFillPaint(linearGradientPaint, dirtyRect);

            for (var i = 0; i < Points.Count; i++)
            {
                var point = Points.ElementAt(i);
                var barHeight = dirtyRect.Height - (dirtyRect.Height * (point.Value / Max) * BarScale);

                //Draw bars
                canvas.FillRectangle(barXAxis, barHeight, BAR_WIDTH, dirtyRect.Height - barHeight);

                //Draw text
                canvas.DrawString(point.Key, barXAxis, barHeight - 20, HorizontalAlignment.Center);
                barXAxis += BAR_WIDTH + 20;
            }

            XAxisScaleOrigin = XAxisScale;
        }

        public float Max;
        public float BarScale = 0.0f;
        public double XAxisScaleOrigin;
        public bool ChartsLoading = true;

        private float _chartWidth;
        private double _xAxisScale;
        private float _firstBarXAxis = 20.0f;
        private Dictionary<string, float> _points;
    }
}