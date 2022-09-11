using System.Collections.Generic;
using KITAB.Products.Domain.Models;
using KITAB.Products.Infra.Paginations;

namespace KITAB.Products.Application.Products
{
    public interface IProductService
    {
        void Insert(ref Product p_product);
        void Update(ref Product p_product);
        void Delete(ref int p_id);
        Product GetById(ref int p_id);
        List<Product> GetAll();
        void ExecuteSQL(ref string p_sql);
        void CreateTable();
        void DropTable();
		PaginationList<Product> GetAllPaginated(string p_filter, string p_order, int p_pageNumber, int p_pageSize);
    }
}
