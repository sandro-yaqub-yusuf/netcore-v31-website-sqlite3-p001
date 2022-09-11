using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using KITAB.Products.Application.Notificators;
using KITAB.Products.Application.Products;
using KITAB.Products.Domain.Models;
using KITAB.Products.Infra.Paginations;
using KITAB.Products.Web.ViewModels;

namespace KITAB.Products.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public ProductsController(IProductService productService, INotificatorService notificatorService, IMapper mapper, IConfiguration config) : base(notificatorService)
        {
            _productService = productService;
            _mapper = mapper;
            _config = config;
        }

        [Route("products/list")]
        [HttpGet]
        public IActionResult Index(int page, string sortField, string currentSortField, string currentSortOrder, string currentFilter, string filter)
        {
            if (_config.GetValue<bool>("StartProjectSettings:CreateTables:Product")) 
            {
                _productService.DropTable();
                _productService.CreateTable();
            }

            if (filter != null)
            {
                page = 1;
            }
            else
            {
                filter = currentFilter;
            }

            if (string.IsNullOrEmpty(sortField))
            {
                currentSortField = "Id";
                currentSortOrder = "ASC";
            }
            else
            {
                if (sortField != "*")
                {
                    if (currentSortField == sortField)
                    {
                        currentSortOrder = (currentSortOrder == "ASC" ? "DESC" : "ASC");
                    }
                    else
                    {
                        currentSortField = sortField;
                        currentSortOrder = "ASC";
                    }
                }
            }

            PaginationList<Product> products = _productService.GetAllPaginated(filter, (currentSortField + " " + currentSortOrder), page, 5);

            ViewBag.Pagination = new PaginationData {
                CurrentPage = (products == null ? 0 : products.PaginationData.CurrentPage),
                HasNext = (products == null ? false : products.PaginationData.HasNext),
                HasPrevious = (products == null ? false : products.PaginationData.HasPrevious),
                PageSize = (products == null ? 0 : products.PaginationData.PageSize),
                TotalCount = (products == null ? 0 : products.PaginationData.TotalCount),
                TotalPages = (products == null ? 0 : products.PaginationData.TotalPages),
                Filter = filter,
                SortField = currentSortField,
                SortOrder = currentSortOrder
            };

            return View(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }
        
        [Route("products/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("products/create")]
        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            // Faz a validação dos dados da camada de apresentação
            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var _imgPrefixo = (Guid.NewGuid() + "_");

                if (!FileUpload(productViewModel.ImageUpload, _imgPrefixo)) return View(productViewModel);

                productViewModel.Image = (_imgPrefixo + productViewModel.ImageUpload.FileName);
            }

            Product product = _mapper.Map<Product>(productViewModel);

            _productService.Insert(ref product);

            // Verifica se há notificações vinda da camada de negócio
            // Caso tenha notificações, devem ser exibidas ao usuário
            if (!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [Route("products/update/{id:int}")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            ProductViewModel _productViewModel = _mapper.Map<ProductViewModel>(_productService.GetById(ref id));

            if (_productViewModel == null) return NotFound();

            return View(_productViewModel);
        }

        [Route("products/update/{id:int}")]
        [HttpPost]
        public IActionResult Update(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            // Faz a validação dos dados da camada de apresentação
            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                string _imgPrefixo = (Guid.NewGuid() + "_");

                if (!FileUpload(productViewModel.ImageUpload, _imgPrefixo)) return View(productViewModel);

                productViewModel.Image = (_imgPrefixo + productViewModel.ImageUpload.FileName);
            }

            Product product = _mapper.Map<Product>(productViewModel);

            _productService.Update(ref product);

            // Verifica se há notificações vinda da camada de negócio
            // Caso tenha notificações, devem ser exibidas ao usuário
            if (!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [Route("products/show/{id:int}")]
        [HttpGet]
        public IActionResult Show(int id)
        {
            ProductViewModel _productViewModel = _mapper.Map<ProductViewModel>(_productService.GetById(ref id));

            if (_productViewModel == null) return NotFound();

            return View(_productViewModel);
        }

        [Route("products/delete/{id:int}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ProductViewModel _productViewModel = _mapper.Map<ProductViewModel>(_productService.GetById(ref id));

            if (_productViewModel == null) return NotFound();

            return View(_productViewModel);
        }

        [Route("products/delete/{id:int}")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            ProductViewModel _productViewModel = _mapper.Map<ProductViewModel>(_productService.GetById(ref id));

            if (_productViewModel == null) return NotFound();

            _productService.Delete(ref id);

            // Verifica se há notificações vinda da camada de negócio
            // Caso tenha notificações, devem ser exibidas ao usuário
            if (!ValidOperation()) return View(_productViewModel);

            TempData["Sucesso"] = "Produto excluido com sucesso !!!";

            return RedirectToAction("Index");
        }

        private bool FileUpload(IFormFile p_imageUpload, string p_imgPrefix)
        {
            if (p_imageUpload.Length <= 0) return false;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products", p_imgPrefix + p_imageUpload.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome !!!");

                return false;
            }

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                p_imageUpload.CopyTo(stream);
            }

            return true;
        }
    }
}
