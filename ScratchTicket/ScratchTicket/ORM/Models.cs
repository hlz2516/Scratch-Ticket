using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.ORM
{
    public class UserInfo
    {
        [Key]
        public string Account { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public double Capital { get; set; } = 100.00;
    }
}
