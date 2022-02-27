using GradientStop = Microsoft.Maui.Graphics.GradientStop;

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

            const string purple = "#7F2CF6";
            const int POINT_SEGMENT_WIDTH = 100;
            canvas.StrokeColor = Color.FromArgb(purple);
            canvas.FontColor = Color.FromArgb(purple);
            canvas.FontSize = 16;

            //If the slider was moved then change x axis for the first bar
            if (XAxisScale != XAxisScaleOrigin)
                _firstPointXAxis += (float)(XAxisScale - XAxisScaleOrigin) * _lastPointXAxis * -1;

            var pointXAxis = _firstPointXAxis;
            var linearPath = new PathF();

            var transparentMauiPurpleGradientStop = new GradientStop(0.0f, Color.FromRgba(178, 127, 255,125));
            var mauiPurpleGradientStop = new GradientStop(0.27f, Color.FromRgba(235, 222, 255,0));
            var linearGradientPaint = new LinearGradientPaint
            {
                EndPoint = new Point(0, 1),
                StartPoint = new Point(0, 0),
                GradientStops = new GradientStop[] { transparentMauiPurpleGradientStop, mauiPurpleGradientStop }
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

            //Draw line chart
            canvas.DrawPath(linearPath);

            //Connect bottom of the line chart
            linearPath.LineTo(new PointF(_lastPointXAxis, dirtyRect.Height));
            linearPath.LineTo(new PointF(0, dirtyRect.Height));

            linearPath.Close();

            //Fill chart with gradient
            canvas.FillPath(linearPath);

            //Remember selected x axis
            XAxisScaleOrigin = XAxisScale;
        }

        public float Max;
        public double XAxisScaleOrigin;

        private double _xAxisScale;
        private float _lastPointXAxis;
        private float _firstPointXAxis = 0.0f;
        private Dictionary<string, float> _points;
    }
}