using ASPShopBag.Data;
using ASPShopBag.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASPShopBag.Services
{
    public class ImagesBuilder
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string wwwroot;

        public ImagesBuilder (ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwroot = $"{this._hostEnvironment.WebRootPath}";
        }
        
        ///
        public async Task CreateImages(ProductsVM model)
        {
            Product productToDb = new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description
            };
            await _context.Products.AddAsync(productToDb);
            await this._context.SaveChangesAsync();

            //var wwwroot = $"{this._hostEnvironment.WebRootPath}";
            //създаваме папката images, ако не съществува
            Directory.CreateDirectory($"{wwwroot}/ProductImages/");
            var imagePath = Path.Combine(wwwroot, "ProductImages");
            string uniqueFileName = null;
            if (model.ImagePath.Count > 0)
            {
                for (int i = 0; i < model.ImagePath.Count; i++)
                {
                    ProductImages dbImage = new ProductImages()
                    {
                        ProductId = productToDb.Id,
                        Product = productToDb
                    };//id се създава автоматично при създаване на обект
                    if (model.ImagePath[i] != null)
                    {
                        uniqueFileName = dbImage.Id + "_" + model.ImagePath[i].FileName;
                        string filePath = Path.Combine(imagePath, uniqueFileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImagePath[i].CopyToAsync(fileStream);
                        }

                        dbImage.ImagePath = uniqueFileName;
                        await _context.ProductImages.AddAsync(dbImage);
                        await this._context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
