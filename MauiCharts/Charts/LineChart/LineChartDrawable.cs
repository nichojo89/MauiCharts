namespace MauiCharts.Charts
{
    internal class LineChartDrawable : View, IDrawable
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
            set
            {
                _xAxisScale = value;
                OnPropertyChanged();
            }
        }

        public LineChartDrawable() => VerticalOptions = LayoutOptions.FillAndExpand;

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.ResetState();

            _chartWidth = dirtyRect.Width;
            const int POINT_SEGMENT_WIDTH = 100;

            //If the slider was moved then change x axis for the first bar
            if (XAxisScale != XAxisScaleOrigin)
            {
                var xMoved = (XAxisScale - XAxisScaleOrigin) * _chartWidth * -1;
                _firstPointXAxis += (float)xMoved;
            }

            var pointXAxis = _firstPointXAxis;
            var linearPath = new PathF();

            var red = new Microsoft.Maui.Graphics.GradientStop(0.0f, Color.FromRgba(178, 127, 255,125));
            var blue = new Microsoft.Maui.Graphics.GradientStop(0.27f, Color.FromRgba(235, 222, 255,0));
            var linearGradientPaint = new LinearGradientPaint
            {
                //StartColor = Color.FromRgba(178, 127, 255,1),
                //EndColor = Color.FromRgba(178, 127, 255, 0),
               
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new Microsoft.Maui.Graphics.GradientStop[] { red, blue }
            };

            var textPaint = new SolidPaint
            {
                Color = Color.FromHex("#7F2CF6")
            };

            //Generate path
            for (var i = 0; i < Points.Count; i++)
            {
                var point = Points.ElementAt(i);
                var yAxis = dirtyRect.Height - (dirtyRect.Height * (point.Value / Max));

                if (i == 0)
                {
                    linearPath.MoveTo(new PointF(pointXAxis, yAxis));
                }
                else
                {
                    linearPath.LineTo(new PointF(pointXAxis, yAxis));
                }

                //canvas.FillCircle(new PointF(pointXAxis, yAxis), 10);

                var isLastDataPoint = i == Points.Count - 1;

                //Draw text
                //TODO where did MeasureText go?
                var pointText = $"{point.Key}: {point.Value}";
                canvas.DrawString(pointText,
                    pointXAxis + 50,
                    yAxis - 10,
                    i <= Points.Count - 2 
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left);

                //Remember last point x axis
                if (isLastDataPoint)
                    _lastPointXAxis = pointXAxis;

                //Move x axis to next point
                pointXAxis += POINT_SEGMENT_WIDTH + 20;
            }

            //canvas.SetFillPaint(linearGradientPaint, dirtyRect);
            canvas.SetFillPaint(linearGradientPaint, new RectangleF(0.0f, dirtyRect.Height - 100,dirtyRect.Width, dirtyRect.Height - 100));

            //Draw Line
            //gradientPaint.Style = SKPaintStyle.Stroke;
            //gradientPaint.StrokeWidth = 7;
            //canvas.FillPath(linearPath);


            linearPath.LineTo(new PointF(_lastPointXAxis, dirtyRect.Height));
            linearPath.LineTo(new PointF(0, dirtyRect.Height));

            linearPath.Close();


            //gradientPaint.Style = SKPaintStyle.Fill;
            //gradientPaint.Shader = SKShader.CreateLinearGradient(
            //                    new SKPoint(info.Rect.MidX, info.Rect.Top),
            //                    new SKPoint(info.Rect.MidX, info.Rect.Bottom),
            //                    new SKColor[] { mauiPurpleColor, transparentMauiPurpleColor },
            //                    new float[] { 0, 1 },
            //                    SKShaderTileMode.Decal);

            canvas.FillPath(linearPath);

            XAxisScaleOrigin = XAxisScale;
        }

        public float Max;
        public double XAxisScaleOrigin;

        private float _chartWidth;
        private double _xAxisScale;
        private float _lastPointXAxis;
        private float _firstPointXAxis = 0.0f;
        private Dictionary<string, float> _points;
    }
}