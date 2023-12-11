using Cadastre.Other;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizensDto
    {
        [Required]
        [MinLength(Validate.CitizenFirstNameMinLength)]
        [MaxLength(Validate.CitizenFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(Validate.CitizenLastNameMinLength)]
        [MaxLength(Validate.CitizenLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string MaritalStatus { get; set; }

        [Required]
        public int[] Properties { get; set; }
    }
}
