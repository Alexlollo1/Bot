namespace Models
{
    public class ArtistObject
    {
        public Artists artists { get; set; }
    }

    public class Artists
    {
        public Artist[] items { get; set; }
    }

    public class Artist
    {
        public External_Urls external_urls { get; set; }
        public string id { get; set; }
        public Image[] images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
    }
}
