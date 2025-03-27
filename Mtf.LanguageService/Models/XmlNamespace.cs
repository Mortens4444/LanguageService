namespace Mtf.LanguageService.Models
{
    public class XmlNamespace
    {
        public string Prefix { get; set; }
        
        public string Uri { get; set; }        

        public XmlNamespace(string prefix, string uri)
        {
            Prefix = prefix;
            Uri = uri;
        }
    }
}
