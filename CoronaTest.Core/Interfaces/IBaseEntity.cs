
namespace CoronaTest.Core.Interfaces
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
