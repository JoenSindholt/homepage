using System.Collections.Generic;

namespace Homepage.Models
{
    public class ImageSectionViewModel
    {
        public string Title { get; set; }

        public IEnumerable<ImageSubSectionViewModel> SubSections { get; set; }
    }
}