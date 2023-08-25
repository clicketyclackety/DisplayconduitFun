using Rhino.Display;
using Rhino.Geometry;

namespace DisplayConduitFun
{
    internal partial class Conduit
    {
        public interface IAnimatedDrawable
        {
            BoundingBox Bounding { get; }
            void DrawFrame(DisplayPipeline pipe, int frame);
        }

    }
}
