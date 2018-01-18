using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Controle.Models
{
    public class UsuarioTarefasModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Apelido { get; set; }

        [Required]
        [Display(Name ="Nome Completo")]
        public string NomeConpleto { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        public virtual ICollection<TimeModel> Times { get; set; }

    }
}