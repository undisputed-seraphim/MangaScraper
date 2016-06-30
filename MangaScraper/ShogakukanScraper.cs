using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace MangaScraper {
	class ShogakukanScraper : WebMangaScraper {

		private string ISBN_gcode;
		private string VisitID;

		public ShogakukanScraper(Uri uri)
			: base(uri) {
			this.ISBN_gcode = "NO_ISBN";
			this.VisitID = "NO_VISIT_ID";
		}

		public override void Execute() {
			HtmlNodeCollection payload = this.webPage.DocumentNode.SelectSingleNode("//div[@id='book_data_area']").ChildNodes;

			foreach (HtmlNode node in payload) {
				if (!node.HasAttributes)
					continue;

				string dataKey = node.GetAttributeValue("data-key", "NO_DATA_KEY");

				if (dataKey == "isbn") {
					this.ISBN_gcode = node.GetAttributeValue("value", "NO_ISBN");
				}

				if (dataKey == "bookTitle") {
					this.title = node.GetAttributeValue("value", "untitled_manga");
				}

				if (dataKey == "vsid") {
					VisitID = node.GetAttributeValue("value", "NO_VISIT_ID");

					if (VisitID == "NO_VISIT_ID") {
						Console.WriteLine("ERROR: " + VisitID);
						Environment.Exit(1);
					}
				}

				if (dataKey == "imageCodes") {
					string imageCode = node.GetAttributeValue("value", "NO_PAGE_FOUND");
#if DEBUG
					Console.WriteLine("Received imageCode " + imageCode);
#endif
					int pagenum = node.GetAttributeValue("data-pagenum", -1);
					string image64 = HTTPRequester(imageCode);
					Image image = Base64ToImage(image64);
					this.OutputImages.Add(pagenum, image);
				}
			}

			WriteAllImagesToDirectory();
		}

		protected override string HTTPRequester(string imageCode) {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri.GetLeftPart(UriPartial.Authority) + "/imgDeliver?gcode=" + this.ISBN_gcode);
			string parameters = "base64=1&vsid=" + this.VisitID + "&trgCode=" + imageCode;
			byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);

			request.Method = "POST";
			request.Referer = this.uri.ToString();
			request.Accept = "text/plain, */*; q=0.01";
			request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
			request.ContentLength = bytes.Length;

			return RequesterReceiver(request, bytes);
		}
	}
}