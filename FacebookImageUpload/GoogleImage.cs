using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis;
using Google.Apis.Services;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
namespace FacebookImageUpload
{
    class GoogleImage
    {

        public Search googleImageSearch(string apiKey, string searchEngineId, string query, uint start)
        {
            CustomsearchService customSearchService = new CustomsearchService(new BaseClientService.Initializer() { ApiKey = apiKey });
            CseResource.ListRequest listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            listRequest.FileType = "jpg";
            listRequest.Start = start;
            listRequest.ImgSize = CseResource.ListRequest.ImgSizeEnum.Xlarge;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Filter = CseResource.ListRequest.FilterEnum.Value1;
            Search search = listRequest.Execute();
            return search;
        }
        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

    }
}
