using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;
using HtmlAgilityPack;

namespace MangaScraper {
	class ChampionCrossScraper : WebMangaScraper {

		public ChampionCrossScraper(Uri uri) 
			: base(uri) { }

		public override void Execute() {
			HtmlNode header_title = this.webPage.DocumentNode.SelectSingleNode("//title");
			this.title = header_title.InnerText;
			Console.WriteLine("Title: " + this.title);

			HtmlNode payload = this.webPage.DocumentNode.SelectSingleNode("//span[@id='totalPage']");

			int page_count = int.Parse(payload.InnerText);

			for (int i = 0; i < page_count; i++) {
				string new_uri = this.uri.AbsoluteUri + "/" + (i + 1).ToString();
				Console.WriteLine("Retrieving page at " + new_uri);
				Image img = DownloadImage(new_uri);
				this.OutputImages.Add(i + 1, img);
			}
			WriteAllImagesToDirectory();
		}

		protected override string HTTPRequester(string parameter) {
			throw new NotImplementedException();
		}
	}
}
