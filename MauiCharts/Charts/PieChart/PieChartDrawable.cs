using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCharts.Charts
{
    internal class PieChartDrawable : View, IDrawable
    {
        public static readonly BindableProperty PointsProperty = BindableProperty.Create(nameof(Points),
            typeof(Dictionary<string, float>),
            typeof(PieChartDrawable),
            new Dictionary<string, float>());

        public Dictionary<string, float> Points
        {
            get => (Dictionary<string, float>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public PieChartDrawable()
        {
            VerticalOptions = LayoutOptions.FillAndExpand;
        }

        /// <summary>
        /// Converts degrees around a circle to a Point
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="radius"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private PointF PointFromDegrees(float degrees, float radius, RectangleF rect, int padding = 0)
        {
            const int offset = 90;

            var x = (float)(rect.Center.X + (radius + padding) * Math.Cos((degrees - offset) * (Math.PI / 180)));
            var y = (float)(rect.Center.Y + (radius + padding) * Math.Sin((degrees - offset) * (Math.PI / 180)));

            return new PointF(x, y);
        }

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            canvas.ResetState();

            var radius = dirtyRect.Width / 4;
            var center = new PointF(dirtyRect.Center.X, dirtyRect.Center.Y);
            var purple = Color.FromRgba(178, 127, 255, 125);
            var translucent = Color.FromRgba(235, 222, 255, 0);

            //Draw Circle 
            var radialGradientPaint = new RadialGradientPaint
            {
                StartColor = translucent,
                EndColor = purple
                // Center is already (0.5,0.5)
                // Radius is already 0.5
            };
            
            var radialRectangle = new RectangleF(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius,radius * 2, radius * 2);
            canvas.SetFillPaint(radialGradientPaint, radialRectangle);
            canvas.FillCircle(center, radius);

            var scale = 100f / Points.Select(x => x.Value).Sum();

            //Draw first initial line
            canvas.StrokeColor = Colors.White;
            canvas.DrawLine(
                new PointF(center.X, center.Y - radius),
                center);

            var lineDegrees = 0f;
            var textDegrees = 0f;

            canvas.FontColor = Color.FromArgb("#7F2CF6");
            var textRadiusPadding = Convert.ToInt32(dirtyRect.Width / 10);

            //Draw splits into pie using 𝝅
            for (var i = 0; i < Points.Count; i++)
            {
                var point = Points.ElementAt(i);
                lineDegrees += 360 * (point.Value * scale / 100);
                textDegrees += (360 * (point.Value * scale / 100) / 2);

                var lineStartingPoint = PointFromDegrees(lineDegrees, radius, dirtyRect);
                var textPoint = PointFromDegrees(textDegrees, radius, dirtyRect, textRadiusPadding);
                var valuePoint = new PointF(textPoint.X, textPoint.Y + 15);

                canvas.DrawLine(
                    lineStartingPoint,
                    center);

                canvas.DrawString(point.Key,
                    textPoint.X,
                    textPoint.Y,
                    HorizontalAlignment.Center);

                canvas.DrawString(point.Value.ToString(),
                    valuePoint.X,
                    valuePoint.Y,
                   HorizontalAlignment.Center);

                textDegrees += (360 * (point.Value * scale / 100) / 2);
            }
        }
    }
}