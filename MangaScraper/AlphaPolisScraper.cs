using System;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using HtmlAgilityPack;

namespace MangaScraper {
	class AlphaPolisScraper : WebMangaScraper {

		public AlphaPolisScraper(Uri uri) 
			: base(uri) { }

		public override void Execute() {
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
					Console.WriteLine("Received image at " + image_src);
					this.OutputImages.Add(page_num, image);
				}
			}

			WriteAllImagesToDirectory();
		}

		// Simple image downloader.
		protected Image DownloadImage(string url) {
			WebClient client = new WebClient();
			byte[] dat = client.DownloadData(url);
			MemoryStream mstream = new MemoryStream(dat);
			return Image.FromStream(mstream, true, true);
		}

		// Not needed for this.
		protected override string HTTPRequester(string parameter) {
			throw new NotImplementedException();
		}
	}
}
