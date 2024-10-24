using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUygulamaProje1.Models
{
    public class KitapTuru
    {
        [Key]  // Id lerin primary key olması yani benzersiz hepsinin farklı ıdler alması için bunu yazdık hangisinin olmasını istersek keyi onun üst satırına koyuyoruz 
        public int Id { get; set; }

        [Required(ErrorMessage ="Kitap türü alanı boş bırakılamaz!!")]  //not null anlamındadır null olamaz 
        [MaxLength(30)] //max 25 kelimelik olabilir kitap adı
        [DisplayName("Kitap Türü Adı")]
        public string Ad {  get; set; }
    }
}


//Ürünlere ıd verirken "Id" dersek sorun olmaz ama başına sonuna diğerlrinden ayırmak için bir şeyler eklersek <input asp-for="verdiğimiz ıd ismi" hidden/> yazarak hata almanın önüne geçiyoruz hangi sayfada kullanacaksak o sayfanın başına yazmamız yeterli ama genelde Id yi kullanmak daha mantıklı!!