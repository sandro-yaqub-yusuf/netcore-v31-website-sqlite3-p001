using KITAB.Products.Domain.Models;

namespace KITAB.Products.Infra.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        void CreateTable();
        void DropTable();
    }
}
