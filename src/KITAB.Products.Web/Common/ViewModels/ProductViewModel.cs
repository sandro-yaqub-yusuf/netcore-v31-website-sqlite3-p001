using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using KITAB.Products.Web.Extensions;

namespace KITAB.Products.Web.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório...")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres...", MinimumLength = 2)]
        [RegularExpression(@"[^~""'`´!@#$%^&*()_={}[\]:;,.<>+\/?]+", ErrorMessage = "Alguns tipos de caracteres não são aceitos no campo {0}...")]
        [DisplayName("Produto")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório...")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres...", MinimumLength = 2)]
        [RegularExpression(@"[^~""'`´!@#$%^&*()_={}[\]:;,<>+\/?]+", ErrorMessage = "Alguns tipos de caracteres não são aceitos no campo {0}...")]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        [DisplayName("Imagem do Product")]
        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório...")]
        [DisplayName("Qtde. Estoque")]
        public int Inventory { get; set; }

        [Coin]
        [Required(ErrorMessage = "O campo {0} é obrigatório...")]
        [DisplayName("Preço Custo")]
        public decimal CostPrice { get; set; }

        [Coin]
        [Required(ErrorMessage = "O campo {0} é obrigatório...")]
        [DisplayName("Preço Venda")]
        public decimal SalePrice { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [DisplayName("Situação")]
        public string Status { get; set; }
    }
}
