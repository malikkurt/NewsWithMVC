using Microsoft.AspNetCore.Mvc;
using NewsWithMVC.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace NewsWithMVC.Controllers
{
    public class NewsController : Controller
    {
        private HttpClient _httpClient;
        string apiKey = "apikey";
        string translatorApıKey = "translatorapıkey"; 

        public NewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(string country, string tags, string language)
        {
            country = string.IsNullOrEmpty(country) ? "tr" : country;
            tags = string.IsNullOrEmpty(tags) ? "general" : tags;
            language = string.IsNullOrEmpty(language) ? "tr" : language; 

            string requestUrl = $"https://api.collectapi.com/news/getNews?country={country}&tag={tags}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"apikey {apiKey}");

            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var newsApiResponse = JsonConvert.DeserializeObject<NewsApiResponse>(jsonResponse);

                foreach (var news in newsApiResponse.Result)
                {
                    news.name = WebUtility.HtmlDecode(await TranslateText(news.name, language)); 
                    news.description = WebUtility.HtmlDecode (await TranslateText(news.description, language)); 
                }
                return View(newsApiResponse.Result);
            }
            else
            {
                return View("Error");
            }
        }

        private async Task<string> TranslateText(string text, string targetLanguage)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var requestUrl = $"https://translation.googleapis.com/language/translate/v2?key={translatorApıKey}";
            var requestBody = new
            {
                q = text,
                target = targetLanguage
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic translationResponse = JsonConvert.DeserializeObject(jsonResponse);

                return translationResponse.data.translations[0].translatedText;
            }
            else
            {
                return text; 
            }
        }
    }
}
