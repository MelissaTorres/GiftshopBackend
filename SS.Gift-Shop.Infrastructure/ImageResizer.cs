using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class ImageResizer : IImageResizer
    {
        private readonly IImageEncoder _encoder = new JpegEncoder();
        public void Resize(Stream input, Stream output, int maxWidth, int maxHeight)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth));
            }

            if (maxHeight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHeight));
            }

            using (var image = Image.Load(input))
            {
                var ratio = CalculateRatio(image, maxWidth, maxHeight);

                if (ratio < 1)
                {
                    var newWidth = Convert.ToInt32(image.Width * ratio);
                    var newHeight = Convert.ToInt32(image.Height * ratio);
                    image.Mutate(x => x.Resize(newWidth, newHeight));

                }

                image.Save(output, _encoder);
            }
        }

        private static double CalculateRatio(IImageInfo image, int maxWidth, int maxHeight)
        {
            double widthRatio = 1;
            double heightRatio = 1;

            if (image.Width > maxWidth)
            {
                widthRatio = (double)maxWidth / image.Width;
            }

            if (image.Height > maxHeight)
            {
                heightRatio = (double)maxHeight / image.Height;
            }

            double ratio = Math.Min(widthRatio, heightRatio);
            return ratio;
        }
    }
}
