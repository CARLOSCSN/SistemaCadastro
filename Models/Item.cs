using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCadastro.Models
{
    public class Item : IEntity
    {
        [Column("ItemID")]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Tipo { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Valor { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime DataInclusao { get; set; }

        public int? LoginID { get; set; }
        public string LoginNome { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Login Login { get; set; }
    }
}