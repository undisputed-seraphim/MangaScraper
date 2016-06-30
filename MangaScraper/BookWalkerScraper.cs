using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaScraper {
	class BookWalkerScraper : WebMangaScraper {

		private string UUID;

		public BookWalkerScraper( Uri uri )
			: base( uri ) {
				this.UUID = this.uri.Segments[1];
				if (this.UUID[this.UUID.Length - 1] == '/') {
					this.UUID = this.UUID.Substring( 0, this.UUID.Length - 1 );
				}

				
		}

		public override void Execute() {
			throw new NotImplementedException();
		}

		protected override string HTTPRequester( string parameter ) {
			throw new NotImplementedException();
		}
	}
}
