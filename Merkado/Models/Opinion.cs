using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Merkado.Models
{
    public class Opinion
    {
        public int OpinionId { get; set; }
        public string Comment { get; set; }
        [Range (1,5)]
        public int Rate { get; set; }
        public string ReviewerId { get; set; }

        [NotMapped]
        public string ReviewerName { get; set; }
    }
}
