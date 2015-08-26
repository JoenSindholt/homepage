using System;
using System.Collections.Generic;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;

namespace Homepage.Managers
{
    public class ThumbnailManager
    {
        private string _baseImagePath = @"C:\Users\Joen\Pictures";

        private dynamic _folders = new
        {
            Years = new List<dynamic>
            {
                new
                {
                    Year = 2015,
                    Folders = new List<string>
                    {
                        "Kroatien",
                        "Sommerland",
                        "Alexander 9 år",
                        "Sverige",
                        "Diverse"
                    }
                }
            }
        };

        public void EnsureThumbnailsUpToDate()
        {
            var fullSizeFolders = GetFullSizeFolders();

            foreach (var fullsizeFolder in fullSizeFolders)
            {
                string thumbFolderPath = GetThumbFolderPath(fullsizeFolder);
                if (!Directory.Exists(thumbFolderPath))
                {
                    CreateThumbsForFolder(fullsizeFolder);
                }
            }
        }

        private void CreateThumbsForFolder(string fullSizeFolder)
        {
            var files = new DirectoryInfo(fullSizeFolder).GetFiles("*.jpg");
            foreach (FileInfo file in files)
            {
                string thumbFilePath = GetThumbFolderPath(fullSizeFolder) + "\\" + file.Name;
                string largeFilePath = GetLargeFolderPath(fullSizeFolder) + "\\" + file.Name;

                if (!File.Exists(thumbFilePath))
                {
                    using (ImageFactory imageFactory = new ImageFactory())
                    {
                        imageFactory.Load(file.FullName);

                        ResizeToSquare(320, imageFactory);

                        imageFactory.AutoRotate();

                        imageFactory.Save(thumbFilePath);
                    }
                }

                if (!File.Exists(largeFilePath))
                {
                    using (ImageFactory imageFactory = new ImageFactory())
                    {
                        imageFactory.Load(file.FullName);

                        ResizeWithAspectRatio(1600, imageFactory);

                        imageFactory.AutoRotate();

                        imageFactory.Save(largeFilePath);
                    }
                }
            }
        }

        private void ResizeToSquare(int width, ImageFactory f)
        {
            var resizeLayer = new ResizeLayer(new System.Drawing.Size(width, width), ResizeMode.Crop, AnchorPosition.Center);
            f.Resize(resizeLayer);
        }

        private void ResizeWithAspectRatio(int width, ImageFactory f)
        {
            int cropWidth = 0;
            int cropHeight = 0;
            if (f.Image.Width > f.Image.Height)
            {
                // landscape
                cropWidth = width;
                cropHeight = (f.Image.Height / f.Image.Width) * width;
            }
            else
            {
                // portrait
                cropHeight = width;
                cropWidth = (f.Image.Width / f.Image.Height) * width;
            }

            f.Resize(new System.Drawing.Size(cropWidth, cropHeight));
        }

        private IEnumerable<string> GetFullSizeFolders()
        {
            foreach (var year in _folders.Years)
            {
                foreach (var folder in year.Folders)
                {
                    yield return Path.Combine(_baseImagePath, year.Year.ToString(), folder);
                }
            }
        }

        private string GetThumbFolderPath(string fullsizeFolder)
        {
            string relPath = fullsizeFolder.Replace(_baseImagePath, "");
            string thumbPath = @"C:\Users\Joen\Documents\Projects\Homepage\Homepage\imagecache\thumbs" + relPath;
            return thumbPath;
        }

        private string GetLargeFolderPath(string fullsizeFolder)
        {
            string relPath = fullsizeFolder.Replace(_baseImagePath, "");
            string thumbPath = @"C:\Users\Joen\Documents\Projects\Homepage\Homepage\imagecache\large" + relPath;
            return thumbPath;
        }
    }
}