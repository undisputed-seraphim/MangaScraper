using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MangaScraper {
	class ComShoScraper : WebMangaScraper {

		public ComShoScraper(Uri uri) 
			: base(uri) { }

		public override void Execute() {
			HtmlNodeCollection payload = this.webPage.DocumentNode.SelectSingleNode("//div[@id='stock']").ChildNodes;

		}

		protected override string HTTPRequester(string parameter) {
			throw new NotImplementedException();
		}
	}
}
