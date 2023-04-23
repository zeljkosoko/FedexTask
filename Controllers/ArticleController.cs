using Microsoft.AspNetCore.Mvc;
using System;
using FedexTask.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace FedexTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleDbContext _context;
        public ArticleController(ArticleDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            Article article = _context.Article.Find(id);

            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost]
        public IActionResult PostArticle([FromBody] JObject data)
        {
            string stringData = JsonConvert.SerializeObject(data);
            Article article = JsonConvert.DeserializeObject<Article>(stringData);

            if (String.IsNullOrEmpty(article.Name) || article.Name.Length > 50)
                return BadRequest("Name length is between 1-50 characters.");
            
            if (String.IsNullOrEmpty(article.Description) || article.Description.Length > 50)
                return BadRequest("Description length is between 1-50 characters");

            if(article.Price <= 0)
                return BadRequest("Price must be greater than zero.");

            _context.Article.Add(article);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Article is not saved in db.");
            }
            catch (Exception)
            {
                return BadRequest("Application execution error.");
            }

            return Ok(article);
        }
    }
}
