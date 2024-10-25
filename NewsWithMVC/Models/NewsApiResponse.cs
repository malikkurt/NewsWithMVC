namespace NewsWithMVC.Models
{
    public class NewsApiResponse
    {
        public bool Success { get; set; }
        public List<News> Result { get; set; }
    }
}
