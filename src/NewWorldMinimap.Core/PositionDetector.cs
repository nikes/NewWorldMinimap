using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using NewWorldMinimap.Core.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TesserNet;

namespace NewWorldMinimap.Core
{
    /// <summary>
    /// Provides logic for performing OCR to find the position of the player.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class PositionDetector : IDisposable
    {
        private const int XOffset = 277;
        private const int YOffset = 18;
        private const int TextWidth = 277;
        private const int TextHeight = 18;
        private const int MaxCounter = 5;

        private static readonly Regex PosRegex = new Regex(@"(\d+ \d+) (\d+ \d+)", RegexOptions.Compiled);

        private readonly ITesseract tesseract = new TesseractPool(new TesseractOptions
        {
            PageSegmentation = PageSegmentation.Line,
            Numeric = true,
            Whitelist = "0123456789",
        });

        private bool disposedValue;
        private float lastX;
        private float lastY;
        private int counter = int.MaxValue;

        /// <summary>
        /// Finalizes an instance of the <see cref="PositionDetector"/> class.
        /// </summary>
        ~PositionDetector()
            => Dispose(false);

        /// <summary>
        /// Tries to get the position from the provided image.
        /// </summary>
        /// <param name="bmp">The image.</param>
        /// <param name="position">The position.</param>
        /// <returns>The found position.</returns>
        public bool TryGetPosition(Image<Rgba32> bmp, out Vector2 position)
        {
            /*
            Image<Rgba32> temp = GetString(bmp, 5, 8);
            //temp.SaveAsPng($"a-{Guid.NewGuid()}.png");

            bmp.Mutate(x => x
                .Crop(new Rectangle(bmp.Width - XOffset, YOffset, TextWidth, TextHeight))
                .Resize(TextWidth * 4, TextHeight * 4)
                .HistogramEqualization()
                .Crop(new Rectangle(0, 2 * 4, TextWidth * 4, 16 * 4))
                .WhiteFilter(0.9f)
                .Dilate(2)
                .Pad(TextWidth * 8, TextHeight * 16, Color.White));

            if (TryGetPositionInternal(bmp, out position))
            {
                return true;
            }
            */

            List<string> results = new List<string>();

            results.Add($"{GetString(bmp, 7, 10)} {GetString(bmp, 12, 14)}");
            results.Add($"{GetString(bmp, 6, 9)} {GetString(bmp, 11, 13)}");
            results.Add($"{GetString(bmp, 5, 9)} {GetString(bmp, 11, 13)}");
            results.Add($"{GetString(bmp, 6, 9)} {GetString(bmp, 11, 13)}");
            results.Add($"{GetString(bmp, 5, 8)} {GetString(bmp, 10, 12)}");
            results.Add($"{GetString(bmp, 4, 8)} {GetString(bmp, 10, 12)}");

            string result = results.Where(x => !x.StartsWith(" ") && !x.EndsWith(" ")).OrderByDescending(x => x.Length).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(result))
            {
                string[] parts = result.Split(' ');
                position = new Vector2(int.Parse(parts[0]), int.Parse(parts[1]));
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    tesseract.Dispose();
                }

                disposedValue = true;
            }
        }

        private const int Characters = 33;
        private const int CharHeight = 14;
        private const int CharWidth = 9;
        private const int Scale = 4;

        /// <summary>
        /// Tries to get the position from the provided image.
        /// </summary>
        /// <param name="bmp">The image.</param>
        /// <param name="position">The position.</param>
        /// <returns>The found position.</returns>
        private Image<Rgba32> GetString(Image<Rgba32> img)
        {
            Image<Rgba32> combined = new Image<Rgba32>(Characters * CharWidth * Scale, CharHeight * Scale);
            combined.Mutate(c =>
            {
                c.BackgroundColor(Color.White);

                for (int i = 5; i <= 8; i++)
                {
                    using Image<Rgba32> charImg = GetChar(img, i);
                    c.DrawImage(charImg, new Point(CharWidth * Scale * i), 0, 1);
                }
            });

            return combined;
        }

        private string? GetString(Image<Rgba32> img, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex + 1;

            Image<Rgba32> result = new Image<Rgba32>(length * CharWidth * Scale, CharHeight * Scale);

            result.Mutate(c =>
            {
                c.BackgroundColor(Color.White);

                for (int i = 0; i < length; i++)
                {
                    using Image<Rgba32> charImg = GetChar(img, i + startIndex);
                    c.DrawImage(charImg, CharWidth * Scale * i, 0);
                }
            });

            Image<Rgba32> temp = result.Clone(c => c.Pad(result.Width * 3, result.Height * 3, Color.White));
            string read = tesseract.Read(temp).Trim();
            temp.SaveAsPng($"a-{read}.png");

            if (read.Length != length)
            {
                return null;
            }

            return read;
        }

        private Image<Rgba32> GetChar(Image<Rgba32> img, int index)
        {
            int invertedIndex = Characters - index;
            int y = 21;

            int x = img.Width - 4 - (CharWidth * invertedIndex);

            Image<Rgba32> cimg = img.Clone(c => c
                .Crop(new Rectangle(x, y, CharWidth, CharHeight))
                .Resize(CharWidth * Scale, CharHeight * Scale)
                .HistogramEqualization()
                .WhiteFilter(0.8f)
                .Dilate(1));

            return cimg;
        }

        private bool TryGetPositionInternal(Image<Rgba32> bmp, out Vector2 position)
        {
            bmp.Metadata.HorizontalResolution = 300;
            bmp.Metadata.VerticalResolution = 300;

            string text = tesseract.Read(bmp).Trim();
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

            position = default;
            return false;
        }
    }
}
