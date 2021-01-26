using CoronaTest.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CoronaTest.Core.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
