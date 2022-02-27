using SkiaSharp;

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

        public BarChartDrawable()
        {
            VerticalOptions = LayoutOptions.FillAndExpand;
        }

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            const int BAR_WIDTH = 20;

            _chartWidth = dirtyRect.Width;
            //If the slider was moved then change x axis for the first bar
            if (XAxisScale != XAxisScaleOrigin)
            {
                var xMoved = (XAxisScale - XAxisScaleOrigin) * _chartWidth * -1;
                _firstBarXAxis += (float)xMoved;
            }


            var barXAxis = _firstBarXAxis;

            using (var paint = new SKPaint()
            {
                TextSize = 30
            })
            {
                using (var textPaint = new SKPaint
                {
                    TextSize = 30,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = new SKColor(127, 44, 246)
                })
                {
                    var linearGradientPaint = new LinearGradientPaint
                    {
                        StartColor = Color.FromRgb(178, 127, 255),
                        EndColor = Color.FromRgba(178, 127, 255,0),
                        StartPoint = new Point(0.5, 0),
                        EndPoint = new Point(0.5, 1)
                    };

                    canvas.SetFillPaint(linearGradientPaint, dirtyRect);
                    canvas.FontColor = Color.FromArgb("#7F2CF6");

                    for (var i = 0; i < Points.Count; i++)
                    {
                        var point = Points.ElementAt(i);
                        var barHeight = dirtyRect.Height - (dirtyRect.Height * (point.Value / Max) * BarScale);
                        var bar = new RectangleF(barXAxis, barHeight, barXAxis + BAR_WIDTH, dirtyRect.Height);

                        //canvas.SetFillPaint(linearGradientPaint, bar);
                        //Draw bars
                        SolidPaint solidPaint = new SolidPaint(Colors.Silver);
                        var transparentMauiPurpleColor = new Color(178, 127, 255, 0);
                        var mauiPurpleColor = new Color(178, 127, 255);

                        canvas.FillRectangle(barXAxis, barHeight, BAR_WIDTH, dirtyRect.Height - barHeight);

                        //canvas.FillRectangle(barXAxis, barHeight, barXAxis + BAR_WIDTH, dirtyRect.Height);

                        //Draw Text
                        paint.Style = SKPaintStyle.Fill;
                        paint.TextAlign = SKTextAlign.Center;

                        canvas.DrawString(point.Key, barXAxis, barHeight - 20, HorizontalAlignment.Center);
                        barXAxis += BAR_WIDTH + 20;
                    }
                }

                XAxisScaleOrigin = XAxisScale;
            }
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