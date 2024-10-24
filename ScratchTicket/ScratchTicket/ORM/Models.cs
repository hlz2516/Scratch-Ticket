using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.ORM
{
    public enum CardBundleType
    {
        Normal,
        Rare,
        Legend
    }

    public class UserInfo
    {
        [Key]
        public string Account { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public double Capital { get; set; }
    }

    public class CardBundle
    {
        [Key]
        public string Guid { get; set; }
        [Required]
        public CardBundleType CardType { get; set; }
        [Required]
        public int CardsCount { get; set; }
        [Required]
        public double Price { get; set; }
        public string Background { get; set; }
    }

    public class PurchasedCardBundle
    {
        [Key]
        [Column(Order = 1)]
        public string AccountID { get; set; }
        [Key]
        [Column(Order = 2)]
        public string CardBundleID { get; set; }
    }
}
