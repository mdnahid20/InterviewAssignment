namespace MovieStory.Models
{
    public class Movie
    {
        public int Id {  get; set; }   
        public string Title { get; set; }   
        public string Cast { get; set; }
        public string Category { get; set; }

        public string ReleaseDate { get; set; }
        public string Budget { get; set; }

        public bool Favourite { get; set; } = false;
    }
}
