using System.ComponentModel.DataAnnotations;

namespace Weterynarze.Models
{
    public class Vet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Należy podać imie właściciela zwierzęcia.")]
        public string Imie { get; set; }


        [Required(ErrorMessage = "Należy podać nazwisko właściciela zwierzęcia.")]
        public string Nazwisko { get; set; }


        [Required(ErrorMessage = "Należy podać gatunek zwierzęcia.")]
        public string Zwierze { get; set; }


        [Required(ErrorMessage = "Należy podać jakie objawy ma zwierze.")]
        public string Objawy { get; set; }

        [Required(ErrorMessage = "Należy podać adres wizyty.")]
        public string Adres { get; set; }


        [Required(ErrorMessage = "Pole Priorytet jest wymagane.")]
        [Range(1, 10, ErrorMessage = "Priorytet musi zawierać się w przedziale od 0 do 10.")]
        public int Priorytet { get; set; }

        public Vet()
        {


        }





    }
}
