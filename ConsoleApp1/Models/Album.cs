namespace Models
{
    public class AlbumObject
    {
        public Albums albums { get; set; }
    }

    public class Albums
    {
        public Album[] items { get; set; }
    }

    public class Album
    {
        public Artist[] artists { get; set; }
        public External_Urls external_urls { get; set; }
        public string id { get; set; } // id search
        public Image[] images { get; set; }
        public string name { get; set; }
        public string release_date { get; set; }
        public int total_tracks { get; set; }
    }
}
