using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;

namespace MangaScraper {
	public abstract class WebMangaScraper {

		protected Uri uri;
		protected string title;
		protected HtmlDocument webPage;
		protected SortedDictionary<int, Image> OutputImages;
		protected CookieCollection cookies;

		public WebMangaScraper(Uri uri) {
			this.uri = uri;
			this.title = "Untitled";
			this.OutputImages = new SortedDictionary<int, Image>();
		}

		public virtual void LoadHTML() {
			Console.WriteLine("Retrieving page at {0}...", uri.Host);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.CookieContainer = new CookieContainer();
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			this.cookies = response.Cookies;

			if (response.StatusCode != HttpStatusCode.OK) {
				Console.WriteLine("HTTP request failed! Status code = " + response.StatusCode.ToString());
				Environment.Exit(1);
			}

			Stream responseStream = response.GetResponseStream();
			StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8, true);
			string data = responseReader.ReadToEnd();

			responseStream.Close();
			responseReader.Close();

			this.webPage = new HtmlDocument();
			this.webPage.LoadHtml(data);
			Console.WriteLine("Web page received.");
		}

		// Main execution body.
		public abstract void Execute();

		// Optional facility for processing additional web data.
		protected abstract string HTTPRequester(string parameter);

		protected static string RequesterReceiver(WebRequest request, byte[] bytes) {
			Stream rs = request.GetRequestStream();
			rs.Write(bytes, 0, bytes.Length);
			rs.Flush();
			rs.Close();

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode != HttpStatusCode.OK) {
				Console.WriteLine(response.StatusCode);
				// Maybe throw an exception here
			}

			StreamReader reader = new StreamReader(response.GetResponseStream(), true);
			string output = reader.ReadToEnd();
			reader.Close();
			response.GetResponseStream().Close();
			response.Close();

			return output;
		}

		// Saves all images contained in OutputImages to the directory named by the title.
		// Each image is named after its page number.
		protected void WriteAllImagesToDirectory() {

			if (!Directory.Exists(this.title)) {
				Directory.CreateDirectory(this.title);
				Directory.SetCurrentDirectory(this.title);
			} else {
				Directory.SetCurrentDirectory(this.title);
			}

			foreach (KeyValuePair<int, Image> kv in OutputImages) {
				string name = string.Format("{0}.jpg", kv.Key);
				kv.Value.Save(name, ImageFormat.Jpeg);
			}
		}

		public static Image Base64ToImage(string input) {
			byte[] rawbytes = Convert.FromBase64String(input);
			MemoryStream stream = new MemoryStream(rawbytes);
			return Image.FromStream(stream);
		}
	}
}