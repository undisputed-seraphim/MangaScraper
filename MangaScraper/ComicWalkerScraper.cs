using System;
using System.Web;
using System.Net;
using HtmlAgilityPack;

namespace MangaScraper {
	class ComicWalkerScraper : WebMangaScraper {

		private static readonly int[] BASE_KEY = { 173, 43, 117, 127, 230, 58, 73, 84, 154, 177, 47, 81, 108, 200, 101, 65 };
		private string cid;

		public ComicWalkerScraper( Uri uri )
			: base( uri ) {
			string[] seg = uri.Segments;
			this.cid = seg[3];
		}

		public override void Execute() {
			HtmlNodeCollection payload = this.webPage.DocumentNode.SelectSingleNode( "//li[@class='readableLinkColor']" ).ChildNodes;

			string readerLink = "";
			foreach (HtmlNode node in payload) {
				if (!node.HasAttributes) {
					continue;
				}
				readerLink = "http://" + this.uri.Host + node.GetAttributeValue( "href", "NO_LINK" );
			}
			LoadReaderPage( readerLink );
		}

		protected void LoadReaderPage(string readerPage) {
			if ("" == readerPage) {
				Console.WriteLine( "Comic Walker: Reader link not found! Exiting." );
				Environment.Exit( 1 );
			}

			Console.WriteLine( "Receiving reader page..." );

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create( readerPage );
			request.CookieContainer = new CookieContainer();
			request.CookieContainer.Add( this.cookies );
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			this.cookies.Add( response.Cookies );

			foreach (Cookie cookie in this.cookies) {
				Console.WriteLine( cookie.ToString() );
			}
		}

		protected override string HTTPRequester( string parameter ) {
			return "";
		}
	}
}
