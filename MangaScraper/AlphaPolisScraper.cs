using HtmlAgilityPack;
using System;
using System.Drawing;
using System.Text;
using System.Net;

namespace MangaScraper {
	class AlphaPolisScraper : WebMangaScraper {

		public AlphaPolisScraper(Uri uri) 
			: base(uri) { }

		public override void Execute() {
			//HtmlNode header_title = this.webPage.DocumentNode.SelectSingleNode("//title");
			//this.title = header_title.InnerText;

			HtmlNodeCollection payload = this.webPage.DocumentNode.SelectSingleNode("//div[@id='book']").ChildNodes;

			foreach (HtmlNode node in payload) {
				if (!node.HasAttributes)
					continue;

				string div_page = node.GetAttributeValue("id", "NO_DIV_ID");
				if (!div_page.Contains("page"))
					continue;

				int page_num = node.GetAttributeValue("page", -1);
				foreach (HtmlNode child in node.ChildNodes) {
					string img_class = child.GetAttributeValue("class", "NO_DIV_ID");
					if (!img_class.Contains("manga_image"))
						continue;

					string image_src = child.GetAttributeValue("src", "INVALID_LINK");
					Image image = DownloadImage(image_src);
					Console.WriteLine("Retrieving image at " + image_src);
					this.OutputImages.Add(page_num, image);
				}
			}

			WriteAllImagesToDirectory();
		}

		// Not needed for this.
		protected override string HTTPRequester(string parameter) {
			throw new NotImplementedException();
		}
	}
}
