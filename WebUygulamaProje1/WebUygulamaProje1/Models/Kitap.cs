using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUygulamaProje1.Models
{
    public class Kitap
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string KitapAdi { get; set; }

        public string Tanim { get; set; }

        [Required]
        public string Yazar { get; set; }


        [Required]
        [Range(10,500)]
        public int Fiyat { get; set; }

        [ValidateNever] //vt kaydetmeden önce istene şartları sağlayıp sağlamadığı kontrol edilir
        public int KitapTuruId { get; set; }
        [ForeignKey("KitapTuruId")]


        [ValidateNever]
        public KitapTuru KitapTuru { get; set; }  //foregin key ile tablolar arası bağlantı kurduk

        [ValidateNever]
        public string ResimUrl { get; set; }


    }
}
