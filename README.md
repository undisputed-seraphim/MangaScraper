# MangaScraper
A program that scrapes manga from preview websites.
Written in C#, with Visual Studio 2015, using HtmlAgilityPack.

## Usage
After building, simple call `MangaScraper.exe` followed by the URL of the viewing website.
MangaScraper will download all pages into a folder within the build directory.

Currently, only Comic Shogakukan and AlphaPolis is supported.

## Developer's Information
* Create a subclass of WebMangaScraper.
* Implement the `Execute()` method.
 * Call `this.webPage.DocumentNode` to retrieve the HTML structure of the webpage.
 * Look for tags that contain information necessary to retrieve the pages.
 * Download the manga page images and put it into `OutputImages`: `this.OutputImages.Addl(page_number, image)
 * At the end of `Execute()`, call `WriteAllImagesToDirectory()` to do exactly as the method name says.
* Add a switch in Program.cs to switch for the URL you support.
* 

## Contribution
Feel free to fork or create a pull request.

You may open an issue as well if you'd like me to add support for a particular website. However this project isn't my top priority at the moment.

There are some viewing websites that transmit scrambled images over the Internet. If you know how to unscramble these images, please contacte me by opening an issue with a relevant title, or email me.

## License
Apache 2.0
