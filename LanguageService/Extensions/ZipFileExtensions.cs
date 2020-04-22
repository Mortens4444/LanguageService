using System.IO;
using System.Xml;
using Ionic.Zip;

namespace LanguageService.Extensions
{
	public static class ZipFileExtensions
	{
		public static MemoryStream GetFile(this ZipFile zipFile, string filename)
		{
			var contentZipEntry = zipFile[filename];
			var contentStream = new MemoryStream();
			contentZipEntry.Extract(contentStream);
			contentStream.Seek(0, SeekOrigin.Begin);
			return contentStream;
		}

		public static XmlDocument GetXmlDocument(this ZipFile zipFile, string filename)
		{
			var contentXml = new XmlDocument();
			using (var contentStream = zipFile.GetFile(filename))
			{
				contentXml.Load(contentStream);
			}
			return contentXml;
		}
	}
}
