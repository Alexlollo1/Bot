namespace Models
{
    public class TracksObject
    {
        public Tracks tracks { get; set; }
    }

    public class Tracks
    {
        public Track[] items { get; set; }
    }

    public class Track
    {
        public Album album { get; set; }
        public Artist[] artists { get; set; }
        public External_Urls external_urls { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string uri { get; set; }
    }
    public class External_Urls
    {
        public string spotify { get; set; }
    }
    public class Image
    {
        public string url { get; set; }
    }
}
