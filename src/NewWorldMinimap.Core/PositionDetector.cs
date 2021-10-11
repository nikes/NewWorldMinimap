using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using IronOcr;
using NewWorldMinimap.Core.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace NewWorldMinimap.Core
{
    /// <summary>
    /// Provides logic for performing OCR to find the position of the player.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class PositionDetector : IDisposable
    {
        private const int MaxCounter = 5;

        private readonly IronTesseract tesseract = new()
        {
            Configuration =
            {
                PageSegmentationMode = TesseractPageSegmentationMode.SingleLine,
                WhiteListCharacters =  "[]0123456789 ,.",
            }
        };

        // private readonly ITesseract tesseract = new TesseractPool(new TesseractOptions
        // {
        //      PageSegmentation = PageSegmentation.Line,
        //      Numeric = true,
        //      Whitelist = "[]0123456789 ,.",
        // });

        private static readonly Regex PosRegex = new(@"(\d+ \d+) (\d+ \d+)", RegexOptions.Compiled);
        
        private bool disposedValue;
        private float lastX;
        private float lastY;
        private int counter = int.MaxValue;

        /// <summary>
        /// Tries to get the position from the provided image.
        /// </summary>
        /// <param name="bmp">The image.</param>
        /// <param name="position">The position.</param>
        /// <param name="debugEnabled">Determines whether or not the debug functionality is enabled.</param>
        /// <param name="debugImage">The resulting debug image.</param>
        /// <returns>The found position.</returns>
        public bool TryGetPosition(Image<Rgba32> bmp, out Vector2 position, bool debugEnabled,
            out Image<Rgba32> debugImage)
        {
            if (bmp is null)
            {
                throw new ArgumentNullException(nameof(bmp));
            }

            var image = bmp.Clone();
            image.Mutate(x => x.Resize(image.Width * 4, image.Height * 4));
            
            debugImage = debugEnabled ? image.Clone() : null!;
            
            if (TryGetPositionInternal(image, out position))
            {
                return true;
            }

            position = default;
            return false;
        }

        /// <summary>
        /// Resets the counter.
        /// </summary>
        public void ResetCounter()
            => counter = int.MaxValue;

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        
        private bool TryGetPositionInternal(Image<Rgba32> bmp, out Vector2 position)
        {
            try
            {
                var result = tesseract.Read(bmp.ToBitmap());
                var text = result.Text;
                Console.WriteLine();
                Console.WriteLine("Read: " + text);
                text = Regex.Replace(text, @"[^0-9]+", " ");
                text = Regex.Replace(text, @"\s+", " ").Trim();
                Match m = PosRegex.Match(text);

                if (m.Success)
                {
                    float x = float.Parse(m.Groups[1].Value.Replace(' ', '.'), CultureInfo.InvariantCulture);
                    float y = float.Parse(m.Groups[2].Value.Replace(' ', '.'), CultureInfo.InvariantCulture);

                    x %= 100000;

                    while (x > 14260)
                    {
                        x -= 10000;
                    }

                    y %= 10000;

                    if (counter >= MaxCounter)
                    {
                        counter = 0;
                    }
                    else
                    {
                        if (Math.Abs(lastX - x) > 20 && counter < MaxCounter)
                        {
                            x = lastX;
                            counter++;
                        }

                        if (Math.Abs(lastY - y) > 20 && counter < MaxCounter)
                        {
                            y = lastY;
                            counter++;
                        }
                    }

                    if (x >= 4468 && x <= 14260 && y >= 84 && y <= 9999)
                    {
                        lastX = x;
                        lastY = y;
                        position = new Vector2(x, y);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] ${e.Message}");
            }

            position = default;
            return false;
        }
    }
}
