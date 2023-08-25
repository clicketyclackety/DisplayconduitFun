using Eto.Forms;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DisplayConduitFun
{
    internal partial class Conduit
    {

        private bool enabled;
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

        public List<IAnimatedDrawable> Drawables;

        DateTime LastDraw;
        public int Frame;


        readonly BoundingBox box;
        readonly Line[] lines;

        internal Conduit()
        {
            Enabled = false;

            var xCount = 10;
            var yCount = 20;
            var zCount = 30;

            Drawables = new List<IAnimatedDrawable>(xCount * yCount * zCount);

            var xWidth = 2000;
            var yWidth = 10000;
            var zWidth = 2000;

            var offset = 1000;

            for (int x = 0; x < xCount; x++)
            {
                for(int y = 0; y < yCount; y++)
                {
                    for(int z = 0; z < zCount; z++)
                    {
                        var xOrigin = x * (xWidth + offset);
                        var yOrigin = y * (yWidth + offset);
                        var zOrigin = z * (zWidth + offset);

                        var origin = new Point3d(xOrigin, yOrigin, zOrigin);
                        var end = new Point3d(xOrigin + xWidth, yOrigin + yWidth, zOrigin + zWidth);

                        var bounds = new BoundingBox(origin, end);
                        Drawables.Add(new AnimatedDrawableExample(bounds));
                    }
                }
            }

            Frame = 0;
            LastDraw = DateTime.UtcNow;

            RhinoApp.Idle += RhinoApp_Idle;
        }

        private void RhinoApp_Idle(object sender, EventArgs e)
        {
            if (CheckDraw())
                RhinoDoc.ActiveDoc.Views.Redraw();
        }

        private bool CheckDraw()
        {
            var timeSinceLastCall = DateTime.UtcNow - LastDraw;
            var fps = 5;
            var delay = TimeSpan.FromSeconds(fps / 60);

            if (timeSinceLastCall < delay)
                return false;

            Frame += 1;

            if (Frame >= 100)
                Frame = 0;

            LastDraw = DateTime.UtcNow;
            return true;
        }

        public void CalculateBoundingBox(object sender, CalculateBoundingBoxEventArgs e)
        {
            foreach (var drawable in Drawables)
            {
                e.IncludeBoundingBox(drawable.Bounding);
            }
        }

        public void PostDrawObjects(object sender, DrawEventArgs e)
        {
            foreach (var drawable in Drawables)
            {
                drawable.DrawFrame(e.Display, Frame);
            }
        }

    }
}
