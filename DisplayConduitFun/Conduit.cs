using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Drawing;

namespace DisplayConduitFun
{
    internal class Conduit
    {

        private bool enabled = false;
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;

                    if (enabled)
                    {
                        DisplayPipeline.CalculateBoundingBox += CalculateBoundingBox;
                        DisplayPipeline.PostDrawObjects += PostDrawObjects;
                    }
                    else
                    {
                        DisplayPipeline.CalculateBoundingBox -= CalculateBoundingBox;
                        DisplayPipeline.PostDrawObjects -= PostDrawObjects;
                    }
                }
            }
        }


        readonly BoundingBox box;
        readonly Line[] lines;

        internal Conduit()
        {
            this.Enabled = true;
            box = new BoundingBox(0, 0, 0, 2160, 10000, 2000);
            lines = box.GetEdges();
            RhinoApp.Idle += RhinoApp_Idle;
        }

        double i = 0;
        DateTime LastDraw = DateTime.UtcNow;
        private void RhinoApp_Idle(object sender, EventArgs e)
        {
            var timeSinceLastCall = DateTime.UtcNow - LastDraw;
            var fps = 24;
            var delay = TimeSpan.FromSeconds(fps / 60);

            if (timeSinceLastCall < delay)
                return;

            LastDraw = DateTime.UtcNow;
            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        bool ToggleDirection = false;
        private void DrawLoading(DrawEventArgs e)
        {
            if (i >= 0.98)
                ToggleDirection = true;

            if (i <= 0.01)
                ToggleDirection = false;

            if (ToggleDirection)
                i -= 0.01;
            else
                i += 0.01;

            int trans = (int)(i * 255);
            ColorStop[] stops = new ColorStop[]
                {
                    new ColorStop(Color.FromArgb(50, Color.DeepSkyBlue), 0),
                    new ColorStop(Color.FromArgb(trans, Color.BlueViolet), i),
                    new ColorStop(Color.FromArgb(50, Color.DeepSkyBlue), 1),
                };

            // e.Display.DrawGradientLines(lines, 3, stops, box.PointAt(0, 0, 0.5), box.PointAt(1, 1, 0.5), true, 1f);
            e.Display.DrawLines(lines, Color.DeepSkyBlue, 3);
            e.Display.DrawGradientMesh(Mesh.CreateFromBox(box, 6, 6, 6), stops, box.PointAt(0, 0, 0.5), box.PointAt(1, 1, 0.5), true, 1f);
        }

        public void CalculateBoundingBox(object sender, CalculateBoundingBoxEventArgs e)
        {
            e.IncludeBoundingBox(box);
        }

        public void PostDrawObjects(object sender, DrawEventArgs e)
        {
            DrawLoading(e);
        }

    }
}
