using System.Windows;

namespace Grappachu.SolutionCloner.UI.Util.CommonInternal
{
    internal static class Screen
    {
        public static double Width => SystemParameters.WorkArea.Width;

        public static double Height => SystemParameters.WorkArea.Height;
    }
}