using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    [Route("v1")]
    public class CategoryController : ControllerBase
    {

        //CRUD

        private readonly StoreDataContext _context;

        public CategoryController(StoreDataContext context)
        {
            _context = context;
        }


        [Route("categories")]
        [HttpGet]
        public IEnumerable<Category> Get() => _context
                                              .Categories
                                              .AsNoTracking()
                                              .ToList();    

        [Route("categories/{id}")]
        [HttpGet]
        public Category Get(int id) => _context
                                                    .Categories
                                                    .AsNoTracking()
                                                    .FirstOrDefault(c => c.Id == id);

        [Route("categories/{id}/products")]
        [HttpGet]
        public IEnumerable<Product> GetProducts(int id) => _context
                                                     .Products
                                                     .AsNoTracking()
                                                     .Where(p => p.CategoryId == id)
                                                     .ToList();


        
        [Route("categories")]
        [HttpPost]
        public async Task<Category> Post([FromBody]Category category) 
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;        
        }                   

        [Route("categories")]
        [HttpPut]
        public async Task<Category> Put([FromBody]Category category) 
        {
            _context.Entry<Category>(category).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            
            return category;
        }                                            

        [Route("categories")]
        [HttpDelete]
        public async Task<Category> Delete([FromBody]Category category) 
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
            
            return category;
        }                   
    }
}