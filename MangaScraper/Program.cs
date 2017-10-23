using System;

namespace MangaScraper {
	class Program {

		private const string TAMESHIYO = "spi.tameshiyo.me";					// Sample: http://spi.tameshiyo.me/HENGO01SPI?page=3
		private const string SHOGAKUKAN_TAMESHIYO = "shogakukan.tameshiyo.me";
		private const string ALPHAPOLIS = "www.alphapolis.co.jp";				// Sample: http://www.alphapolis.co.jp/content/sentence/114578/
		private const string CHAMPION_CROSS = "chancro.jp";						// Sample: http://chancro.jp/comics/samurai/10
		private const string CYCOMICS = "cycomi.com";							// Sample: https://cycomi.com/viewer.php?chapter_id=841

		private const string JRAW_NET = "jraw.net";								// http://jraw.net/Manga/common/post/4749002318?word=%E5%A4%A9%E9%87%8E%E3%82%81%E3%81%90%E3%81%BF&type=1

		static void Main(string[] args) {
			if (args.Length == 0) {
				Console.WriteLine("Usage: Enter the URL of a book as parameter.");
				Console.WriteLine("Dumping will begin automagically.");
				return;
			}

			if (args[0] == "-c") {
				System.Drawing.Image image = WebMangaScraper.Base64ToImage(args[1]);
				image.Save("000.jpg");
				return;
			}

			if (args[0] == "-f") {
				String b64str = System.IO.File.ReadAllText(args[1], System.Text.Encoding.ASCII);
				System.Drawing.Image image = WebMangaScraper.Base64ToImage(b64str);
				image.Save("000.jpg");
				return;
			}

			if (args[0] == "-l") {
				String html_string = System.IO.File.ReadAllText(args[1], System.Text.Encoding.UTF8);
				Console.OutputEncoding = System.Text.Encoding.UTF8;
				Console.WriteLine(html_string);
				return;
			}

			Uri uri = new Uri(args[0]);
			WebMangaScraper scraper;
			switch (uri.Host) {
				case SHOGAKUKAN_TAMESHIYO:
				case TAMESHIYO: {
						scraper = new ShogakukanScraper(uri);
						scraper.LoadHTML();
						scraper.Execute();
						break;
					}
				case ALPHAPOLIS: {
						scraper = new AlphaPolisScraper(uri);
						scraper.LoadHTML();
						scraper.Execute();
						break;
					}
				case CHAMPION_CROSS: {
						scraper = new ChampionCrossScraper(uri);
						scraper.LoadHTML();
						scraper.Execute();
						break;
					}
				default: {
						Console.WriteLine("Unsupported website. Test loading of web page only.");
						scraper = new TestPageLoader(uri);
						scraper.LoadHTML();
						scraper.Execute();
						break;
					}
			}
		}
	}
}