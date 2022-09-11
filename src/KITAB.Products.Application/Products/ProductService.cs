using System;
using System.Collections.Generic;
using KITAB.Products.Application.Notificators;
using KITAB.Products.Domain.Models;
using KITAB.Products.Infra.Paginations;
using KITAB.Products.Infra.Products;

namespace KITAB.Products.Application.Products
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, INotificatorService notificatorService) : base(notificatorService)
        {
            _productRepository = productRepository;
        }

        public void Insert(ref Product p_product)
        {
            try
            {
                p_product.CreatedAt = DateTime.Now;
                p_product.UpdatedAt = null;

                _productRepository.Insert(ref p_product);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void Update(ref Product p_product)
        {
            try
            {
                p_product.UpdatedAt = DateTime.Now;

                _productRepository.Update(ref p_product);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void Delete(ref int p_id)
        {
            try
            {
                _productRepository.Delete(ref p_id);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public Product GetById(ref int p_id)
        {
            Product product = null;

            try
            {
                product = _productRepository.GetById(ref p_id);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }

            return product;
        }

        public List<Product> GetAll()
        {
            List<Product> products = null;

            try
            {
                products = _productRepository.GetAll();
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }

            return products;
        }

        public void ExecuteSQL(ref string p_sql)
        {
            try
            {
                _productRepository.ExecuteSQL(ref p_sql);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void CreateTable()
        {
            try
            {
                _productRepository.CreateTable();
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public void DropTable()
        {
            try
            {
                _productRepository.DropTable();
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }
		
        public PaginationList<Product> GetAllPaginated(string p_filter, string p_order, int p_pageNumber, int p_pageSize)
        {
            PaginationList<Product> products = null;

            try
            {
                products = _productRepository.GetAllPaginated(ref p_filter, ref p_order, ref p_pageNumber, ref p_pageSize);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }

            return products;
        }
    }
}
