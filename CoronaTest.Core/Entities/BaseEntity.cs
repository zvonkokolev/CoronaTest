using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaTest.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
