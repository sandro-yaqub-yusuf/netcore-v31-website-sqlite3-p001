using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using KITAB.Products.Domain.Models;
using KITAB.Products.Infra.Paginations;

namespace KITAB.Products.Infra.Products
{
    public class ProductRepository : Repository, IProductRepository
    {
        public void Insert(ref Product p_product)
        {
            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();
                
                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        // Insere o dado do produto na tabela "Product"
                        var sql = "INSERT INTO Product " +
                                  "(Name, Description, Image, Inventory, CostPrice, SalePrice, CreatedAt, UpdatedAt, Status) " +
                                  "VALUES (@Name, @Description, @Image, @Inventory, @CostPrice, @SalePrice, @CreatedAt, @UpdatedAt, @Status);" +
                                  "select last_insert_rowid();";

                        p_product.Id = cnn.Query<int>(sql, p_product).First();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }

                cnn?.Close();
                cnn?.Dispose();
            }
        }

        public void Update(ref Product p_product)
        {
            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        // Altera o dado do produto na tabela "Product"
                        var sql = "UPDATE Product SET " +
                                  "Name = @Name, " +
                                  "Description = @Description, " +
                                  "Image = @Image, " +
                                  "Inventory = @Inventory, " +
                                  "CostPrice = @CostPrice, " +
                                  "SalePrice = @SalePrice, " +
                                  "UpdatedAt = @UpdatedAt, " +
                                  "Status = @Status " +
                                  "WHERE Id = @Id;";

                        cnn.Execute(sql, p_product);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }

                cnn?.Close();
                cnn?.Dispose();
            }
        }

        public void Delete(ref int p_id)
        {
            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        // Exclui o dado do produto na tabela "Product"
                        var sql = "DELETE FROM Product WHERE Id = @p_id;";

                        cnn.Execute(sql, new {p_id});

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }

                cnn?.Close();
                cnn?.Dispose();
            }
        }

        public Product GetById(ref int p_id)
        {
            Product product = null;

            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                try
                {
                    cnn.Open();

                    var sql = "SELECT * FROM Product WHERE Id = @p_id;";

                    product = cnn.Query<Product>(sql, new {p_id}).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                cnn?.Close();
                cnn?.Dispose();
            }

            return product;
        }

        public List<Product> GetAll()
        {
            List<Product> products = null;

            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                try
                {
                    cnn.Open();

                    var sql = "SELECT * FROM Product ORDER BY Id ASC;";

                    products = cnn.Query<Product>(sql).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                cnn?.Close();
                cnn?.Dispose();
            }

            return products;
        }

        public void ExecuteSQL(ref string p_sql)
        {
            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        // Executa as instruções sql na tabela "Product"
                        cnn.Execute(p_sql);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }

                cnn?.Close();
                cnn?.Dispose();
            }
        }

        public void CreateTable()
        {
            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();
                
                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        // Cria a tabela "Produto"
                        var sql = "CREATE TABLE IF NOT EXISTS Product (" +
                                  "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                  "Name VARCHAR(200) NOT NULL, " +
                                  "Description VARCHAR(1000) NOT NULL, " +
                                  "Image VARCHAR(200) NOT NULL, " +
                                  "Inventory INTEGER NOT NULL, " +
                                  "CostPrice DOUBLE NOT NULL, " +
                                  "SalePrice DOUBLE NOT NULL, " +
                                  "CreatedAt DATETIME NOT NULL, " +
                                  "UpdatedAt DATETIME NULL," +
                                  "Status CHAR(1) NOT NULL DEFAULT 'A');";

                        cnn.Execute(sql);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }

                cnn?.Close();
                cnn?.Dispose();
            }
        }

        public void DropTable()
        {
            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                cnn.Open();

                using (var transaction = cnn.BeginTransaction())
                {
                    try
                    {
                        // Exclui a tabela "Product"
                        var sql = "DROP TABLE IF EXISTS Product;";

                        cnn.Execute(sql);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }

                cnn?.Close();
                cnn?.Dispose();
            }
        }
		
        public PaginationList<Product> GetAllPaginated(ref string p_filter, ref string p_order, ref int p_pageNumber, ref int p_pageSize)
        {
            List<Product> products = null;

            if (File.Exists(DbFile))
            {
                using var cnn = SimpleDbConnection();

                try
                {
                    cnn.Open();

                    var where = (String.IsNullOrEmpty(p_filter) ? "" : " WHERE Name LIKE '%" + p_filter + "%' ");
                    var order = (String.IsNullOrEmpty(p_order) ? ";" : " ORDER BY " + p_order + ";");
                    var sql = "SELECT * FROM Product" + where + order;

                    products = cnn.Query<Product>(sql).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

				cnn?.Close();
				cnn?.Dispose();
            }

            return PaginationList<Product>.PaginateData(ref products, ref p_pageNumber, ref p_pageSize);
        }
    }
}
