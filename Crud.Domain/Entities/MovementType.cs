
namespace MisCanchas.Domain.Entities
{
    public class MovementType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Movement>? Movements { get; set; }
        public bool Incremental { get; set; }
    }
}
