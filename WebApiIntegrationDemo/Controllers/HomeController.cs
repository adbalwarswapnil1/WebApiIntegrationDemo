using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApiIntegrationDemo.Models;

namespace WebApiIntegrationDemo.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {

            List<Movie> movie_list = new List<Movie>();





            var response = client.GetAsync("http://localhost:5050/api/Movies");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var movies = test.Content.ReadAsAsync<List<Movie>>();
                movies.Wait();
                movie_list = movies.Result;

            }
            return View(movie_list);


        }

        [Route("Home/Index/{id}")]
        [HttpGet]
        public Movie Index(int id)
        {

            //List<Movie> movie_list = new List<Movie>();





            var response = client.GetAsync("http://localhost:5050/api/Movies/"+id);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                //var movies = test.Content.ReadAsAsync<List<Movie>>();
                //movies.Wait();
                //movie_list = movies.Result;
                var result = test.Content.ReadAsAsync<Movie>();
                result.Wait();
                Movie finalResult = result.Result;
                return finalResult;

            }
            return null;
            //return View(movie_list);


        }



        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {

            // Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON. 
            var response = client.PostAsJsonAsync<Movie>("http://localhost:5050/api/Movies", movie);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            var responce = client.DeleteAsync("http://localhost:5050/api/Movies/"+id);
            
            responce.Wait();
            var test = responce.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFound");
        }

        public ActionResult Edit(int id)
        {
            Movie responce = Index(id);
            
            if (responce.Id==id)
            {
                return View(responce);
            }
            return RedirectToAction("NotFound");
        }

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            var responce = client.PutAsJsonAsync<Movie>("http://localhost:5050/api/Movies/" + movie.Id, movie);
            var test = responce.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFound");
        }

        public ActionResult Details(int id)
        {
            Movie responce = Index(id);

            if (responce.Id == id)
            {
                return View(responce);
            }

            return RedirectToAction("NotFound");
        }
        public ActionResult NotFound()
        {
            return View();
        }
    }
}