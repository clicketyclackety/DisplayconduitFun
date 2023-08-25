using Eto.Forms;
using Rhino.Display;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace DisplayConduitFun
{
    internal partial class Conduit
    {
        public sealed class AnimatedDrawableExample : IAnimatedDrawable
        {

            static Dictionary<int, ColorStop[]> Stops;

            public BoundingBox Bounding { get; private set; }
            Line[] lines;
            Point3d start;
            Point3d end;
            Mesh box;

            public AnimatedDrawableExample(BoundingBox bounds)
            {
                Bounding = bounds;
                box = Mesh.CreateFromBox(Bounding, 2, 2, 2);
                lines = Bounding.GetEdges();
                start = Bounding.PointAt(0, 0, 0.5);
                end = Bounding.PointAt(1, 1, 0.5);
            }

            static AnimatedDrawableExample()
            {
                Stops = new Dictionary<int, ColorStop[]>(100);

                for(int i = 0; i < 100; i++)
                {
                    double di = i;
                    int transparency = (int)(di * 2.55);

                    ColorStop[] stops;
                    if (i <= 2)
                    {
                        stops = new ColorStop[]
                        {
                            new ColorStop(Color.FromArgb(transparency, Color.BlueViolet), 0),
                            new ColorStop(Color.FromArgb(50, Color.DeepSkyBlue), 1),
                        };
                    }
                    else if (i >= 98)
                    {
                        stops = new ColorStop[]
                        {
                            new ColorStop(Color.FromArgb(50, Color.DeepSkyBlue), 0),
                            new ColorStop(Color.FromArgb(255, Color.BlueViolet), 1),
                        };
                    }
                    else
                    {
                        stops = new ColorStop[]
                        {
                            new ColorStop(Color.FromArgb(50, Color.DeepSkyBlue), 0),
                            new ColorStop(Color.FromArgb(transparency, Color.BlueViolet), (di + 1) / 100),
                            new ColorStop(Color.FromArgb(50, Color.DeepSkyBlue), 1),
                        };
                    }

                    Stops.Add(i, stops);
                }
            }

            private bool Reverse = false;
            public void DrawFrame(DisplayPipeline pipe, int frame)
            {
                if (Reverse)
                    frame = 100 - frame;

                if (frame >= 100)
                    Reverse = !Reverse;
                
                if (frame == 1)
                    Reverse = !Reverse;


                var stops = Stops[frame];

                pipe.DrawLines(lines, Color.DeepSkyBlue, 3);
                pipe.DrawGradientMesh(box, stops, start, end, true, 1f);
            }
            
        }

    }
}
