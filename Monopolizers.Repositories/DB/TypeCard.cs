using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopolizers.Repository.DB
{
    [Table("TypeCard")]
    public class TypeCard
    {
        [Key]
        public int TypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
