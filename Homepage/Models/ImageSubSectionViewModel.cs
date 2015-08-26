using System.Collections.Generic;

namespace Homepage.Models
{
    public class ImageSubSectionViewModel
    {
        public string Title { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }
    }
}