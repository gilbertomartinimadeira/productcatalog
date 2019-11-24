using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly StoreDataContext _context;

        public ProductController(StoreDataContext context)
        {
            _context = context;
        }

        [Route("v1/Products")]
        [HttpGet]
        public async  Task<IEnumerable<ListProductViewModel>> Get() {

        return await _context
                .Products
                .Include(p => p.Category).Select(p => new ListProductViewModel{
                    Id = p.Id,
                    Category = p.Category.Title,
                    Price = p.Price,
                    Title = p.Title,
                    CategoryId = p.Category.Id

                }).AsNoTracking().ToListAsync();       
                 
        }

        [Route("v1/products/{id}")]
        [HttpGet]
        public async Task<ListProductViewModel> Get(int id){
            return await _context.Products.Where(p => p.Id == id)
                                          .Select(p => new ListProductViewModel{
                                                    Id = p.Id,
                                                    Category = p.Category.Title,
                                                    Price = p.Price,
                                                    Title = p.Title,
                                                    CategoryId = p.Category.Id
            }).FirstOrDefaultAsync();
        }

        [Route("v1/products")]
        [HttpPost]
        public async Task<ResultViewModel> Post([FromBody]EditorProductViewModel model){

            model.Validate();        

            if(model.IsValid){
                return new ResultViewModel {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                }    
            }

            var product = new Product(){
                 Title = model.Title,
                 CategoryId = model.CategoryId,
                 CreateDate = DateTime.Now,
                 Description = model.Description,
                 Image = model.Image,
                 LastUpdateDate = DateTime.Now,
                 Price = model.Price,
                 Quantity = model.Quantity            


            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            

            return await Task.FromResult(new ResultViewModel(){
                Success = true,
                Message = "Produto cadastrado com sucesso",
                Data = product
            });
        }

         [Route("v1/products")]
        [HttpPut]
        public async Task<ResultViewModel> Put([FromBody]EditorProductViewModel model){

            model.Validate();        

            if(model.IsValid){
                return new ResultViewModel {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };    
            }

            var product = _context.Products.Find(model.Id);

            if(product == null){
                return new ResultViewModel(){
                    Success = false,
                    Data = model,
                    Message = "Produto não encontrado"
                };
            }
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.CreateDate = DateTime.Now;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            
            _context.Entry<Product>(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            

            return await Task.FromResult(new ResultViewModel(){
                Success = true,
                Message = "Produto atualizado com sucesso",
                Data = product
            });
        }
    }
}