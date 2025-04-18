﻿using Microsoft.AspNetCore.Mvc;
using asp.net_Veikals.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace asp.net_Veikals.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Display the Add Product form
        [HttpGet]
        public IActionResult AddProduct()
        {
            System.Diagnostics.Debug.WriteLine("Entering AddProduct GET method.");
            var categories = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                })
                .ToList();
            System.Diagnostics.Debug.WriteLine($"Categories List Count: {categories.Count}");
            foreach (var category in categories)
            {
                System.Diagnostics.Debug.WriteLine($"Category: {category.Value} - {category.Text}");
            }
            if (categories.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No categories found.");
            }
            ViewData["Categories"] = categories;
            System.Diagnostics.Debug.WriteLine("ViewData['Categories'] has been set.");

            return View("~/Views/Home/Product/add_product.cshtml");
        }



        // Handle the product form submission (POST)
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductFormViewModel model, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (model.Id > 0)
                    {
                        // EDIT
                        var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == model.Id);
                        if (product == null) return NotFound();

                        product.Name = model.Name;
                        product.ShortDesc = model.ShortDesc;
                        product.LongDesc = model.LongDesc;
                        product.Price = model.Price;
                        product.Category = (Category)Enum.Parse(typeof(Category), model.Category);
                        product.UpdatedAt = DateTime.UtcNow;

                        // Replace images if new ones are uploaded
                        if (images != null && images.Any(i => i != null && i.Length > 0))
                        {
                            var existingImages = _context.Images.Where(i => i.ProductId == product.Id);
                            _context.Images.RemoveRange(existingImages);

                            var mainImageUrl = await SaveImage(images.First());
                            _context.Images.Add(new Image
                            {
                                Url = mainImageUrl,
                                IsMainImage = true,
                                ProductId = product.Id
                            });

                            for (int i = 1; i < images.Count && i < 3; i++)
                            {
                                var imageUrl = await SaveImage(images[i]);
                                _context.Images.Add(new Image
                                {
                                    Url = imageUrl,
                                    IsMainImage = false,
                                    ProductId = product.Id
                                });
                            }
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction("admin_acc", "Home");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Model is valid.");
                        System.Diagnostics.Debug.WriteLine($"Name: {model.Name}");
                        System.Diagnostics.Debug.WriteLine($"ShortDesc: {model.ShortDesc}");
                        System.Diagnostics.Debug.WriteLine($"LongDesc: {model.LongDesc}");
                        System.Diagnostics.Debug.WriteLine($"Price: {model.Price}");
                        System.Diagnostics.Debug.WriteLine($"Category: {model.Category}");
                        System.Diagnostics.Debug.WriteLine($"Images count: {images?.Count}");

                        var product = new Product
                        {

                            Name = model.Name,
                            ShortDesc = model.ShortDesc,
                            LongDesc = model.LongDesc,
                            Price = model.Price,
                            Category = (Category)Enum.Parse(typeof(Category), model.Category),
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            Slug = GenerateSlug(model.Name)
                        };

                        _context.Products.Add(product);
                        await _context.SaveChangesAsync();

                        System.Diagnostics.Debug.WriteLine($"Product saved with ID: {product.Id}");

                        var mainImageUrl = await SaveImage(images.FirstOrDefault());
                        System.Diagnostics.Debug.WriteLine($"Main image URL: {mainImageUrl}");

                        var mainImage = new Image
                        {
                            Url = mainImageUrl,
                            IsMainImage = true,
                            ProductId = product.Id
                        };
                        _context.Images.Add(mainImage);

                        for (int i = 1; i < images.Count && i < 3; i++)
                        {
                            var imageUrl = await SaveImage(images[i]);
                            System.Diagnostics.Debug.WriteLine($"Additional image {i} URL: {imageUrl}");

                            var image = new Image
                            {
                                Url = imageUrl,
                                IsMainImage = false,
                                ProductId = product.Id
                            };
                            _context.Images.Add(image);
                        }

                        await _context.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine("Images saved successfully.");

                        return RedirectToAction("admin_acc", "Home");
                    }
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        if (ex.InnerException.InnerException != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Deeper Inner: {ex.InnerException.InnerException.Message}");
                        }
                    }
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Model is invalid.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState Error for {key}: {error.ErrorMessage}");
                    }
                }
            }

            return View("~/Views/Home/Product/add_product.cshtml", model);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                System.Diagnostics.Debug.WriteLine($"ModelState Error for image is NULL");
                return null;

            }

            var filePath = Path.Combine("wwwroot/images", Guid.NewGuid() + Path.GetExtension(imageFile.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            System.Diagnostics.Debug.WriteLine($"Before image return");

            return "/images/" + Path.GetFileName(filePath);
        }


        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var categories = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                })
                .ToList();

            ViewData["Categories"] = categories;
            ViewData["MainImageUrl"] = product.Images.FirstOrDefault(i => i.IsMainImage)?.Url;
            ViewData["AdditionalImages"] = product.Images.Where(i => !i.IsMainImage).Select(i => i.Url).ToList();

            var model = new ProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ShortDesc = product.ShortDesc,
                LongDesc = product.LongDesc,
                Price = product.Price,
                Category = product.Category.ToString(),
            };

            return View("~/Views/Home/Product/add_product.cshtml", model);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Delete image files from disk
            if (product.Images != null && product.Images.Any())
            {
                foreach (var image in product.Images)
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Remove image records from DB
                _context.Images.RemoveRange(product.Images);
            }

            // Delete the product itself
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("admin_acc", "Home");
        }



        string GenerateSlug(string text)
        {
            return text.ToLower()
                .Replace(" ", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace(";", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("/", "")
                .Replace("\\", "")
                .Replace("\"", "")
                .Replace("'", "")
                .Replace("&", "and");
        }






        public IActionResult ShopCategory()
        {
            System.Diagnostics.Debug.WriteLine("State 1: Entering ShopCategory method");

            var products = _context.Products
                .Include(p => p.Images)  
                .ToList();

            System.Diagnostics.Debug.WriteLine($"State 2: Products count = {products.Count}");
            foreach (var product in products)
            {
                System.Diagnostics.Debug.WriteLine($"Product: {product.Name}, Category: {product.Category}, Images count: {product.Images?.Count}");
            }

            var productViewModels = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category.ToString(),
                MainImageUrl = product.Images != null && product.Images.Any()
                                ? product.Images.FirstOrDefault(img => img.IsMainImage)?.Url ?? "/images/default-image.jpg"
                                : "/images/default-image.jpg" 
            }).ToList();

            System.Diagnostics.Debug.WriteLine($"State 3: ViewModels count = {productViewModels.Count}");
            foreach (var viewModel in productViewModels)
            {
                System.Diagnostics.Debug.WriteLine($"ViewModel: {viewModel.Name}, MainImageUrl: {viewModel.MainImageUrl}");
            }
            return View("~/Views/Home/Product/s_categ.cshtml", productViewModels);
        }



        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Name,
                    p.ShortDesc,
                    p.LongDesc,
                    p.Price,
                    Images = p.Images.OrderByDescending(img => img.IsMainImage).Select(img => new ImageViewModel
                    {
                        Url = img.Url,
                        IsMainImage = img.IsMainImage
                    }).ToList(),
                                        p.Category
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailsViewModel
            {
                Name = product.Name,
                ShortDesc = product.ShortDesc,
                LongtDesc = product.LongDesc,
                Price = product.Price,
                Images = product.Images,
                CategoryName = Enum.GetName(typeof(Category), product.Category)
            };
            return View("~/Views/Home/Product/product_details.cshtml", viewModel);
        }




    }
}