using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using NewWorldMinimap.Core.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Point = SixLabors.ImageSharp.Point;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace NewWorldMinimap.Util
{
    /// <summary>
    /// Provides logic for getting a screenshot of an image.
    /// </summary>
    public static class ScreenGrabber
    {
        /// <summary>
        /// Gets the screen count.
        /// </summary>
        /// <returns>The number of screens.</returns>
        public static int ScreenCount => Screen.AllScreens.Length;

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="screenIndex">Index of the screen.</param>
        /// <returns>The taken screenshot.</returns>
        public static Image<Rgba32> TakeScreenshot(int width, int height, Point offset, int screenIndex = 0)
        {
            Screen[] screens = Screen.AllScreens;
            Screen screen = screenIndex >= 0 && screenIndex < screens.Length ? screens[screenIndex] : Screen.PrimaryScreen;

            var rect = User32.GetActiveWindowRect();
            var zone = new Rectangle(rect.Right - offset.X, rect.Top + offset.Y, width, height);
            
            using Bitmap bmp = new(zone.Width, zone.Height, PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(zone.Left, zone.Top, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
            }

            return bmp.ToImageSharp();
        }

        /// <summary>
        /// Gets the index of the primary screen.
        /// </summary>
        /// <returns>The index of the primary screen.</returns>
        [SuppressMessage("Design", "CA1024", Justification = "Performs a computation.")]
        public static int GetPrimaryScreenIndex()
        {
            Screen[] screens = Screen.AllScreens;

            for (var i = 0; i < screens.Length; i++)
            {
                if (screens[i].Equals(Screen.PrimaryScreen))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
