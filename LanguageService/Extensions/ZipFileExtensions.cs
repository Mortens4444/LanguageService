using System.IO;
using System.IO.Compression;
using System.Xml;

namespace LanguageService.Extensions
{
    public static class ZipFileExtensions
    {
        public static MemoryStream GetFile(this ZipArchive zipArchive, string filename)
        {
            var zipEntry = zipArchive.GetEntry(filename);
            if (zipEntry == null)
            {
                throw new FileNotFoundException($"File '{filename}' not found in the ZIP archive.");
            }

            var contentStream = new MemoryStream();
            using (var entryStream = zipEntry.Open())
            {
                entryStream.CopyTo(contentStream);
            }

            contentStream.Seek(0, SeekOrigin.Begin);
            return contentStream;
        }

        public static XmlDocument GetXmlDocument(this ZipArchive zipArchive, string filename)
        {
            var contentXml = new XmlDocument();
            using (var stream = zipArchive.GetFile(filename))
            {
                contentXml.Load(stream);
            }
            return contentXml;
        }
	}
}
