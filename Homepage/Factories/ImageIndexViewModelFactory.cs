using System.IO;
using System.Linq;
using Homepage.Config;
using Homepage.Models;

namespace Homepage.Factories
{
    public class ImageIndexViewModelFactory
    {
        public ImageIndexViewModel BuildImageIndexViewModel(Configuration config)
        {
            string thumbPath = Path.Combine("imagecache", "thumbs");
            string largePath = Path.Combine("imagecache", "large");

            ImageIndexViewModel model = new ImageIndexViewModel
            {
                Sections = config.Folders.Select(f => new ImageSectionViewModel
                {
                    Title = f.Title,
                    SubSections = f.SubFolders.Select(sf => new ImageSubSectionViewModel
                    {
                        Title = sf,
                        Images = new DirectoryInfo(Path.Combine(config.BasePath, thumbPath, f.Title, sf)).GetFiles("*.jpg").Select(file => new ImageViewModel
                        {
                            Src = Path.Combine(thumbPath, f.Title, sf, file.Name),
                            LargeSrc = Path.Combine(largePath, f.Title, sf, file.Name)
                        })
                    })
                })
            };

            return model;
        }
    }
}