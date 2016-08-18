using System;

namespace MangaScraper {
	class Program {

		private const string TAMESHIYO = "spi.tameshiyo.me";					// Sample: http://spi.tameshiyo.me/HENGO01SPI?page=3
		private const string SHOGAKUKAN_TAMESHIYO = "shogakukan.tameshiyo.me";
		private const string ALPHAPOLIS = "www.alphapolis.co.jp";				// Sample: http://www.alphapolis.co.jp/content/sentence/114578/
		private const string CHAMPION_CROSS = "chancro.jp";						// Sample: http://chancro.jp/comics/samurai/10
		private const string CYCOMICS = "cycomi.com";							// Sample: https://cycomi.com/viewer.php?chapter_id=841

		static void Main(string[] args) {
			if (args.Length == 0) {
				Console.WriteLine("Usage: Enter the URL of a book as parameter.");
				Console.WriteLine("Dumping will begin automagically.");
				return;
			}

			if (args[0] == "-c") {
				System.Drawing.Image image = WebMangaScraper.Base64ToImage(args[1]);
				image.Save("000.jpg");
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
						Console.WriteLine("Unsupported website.");
						Environment.Exit(1);
						break;
					}
			}
		}
	}
}