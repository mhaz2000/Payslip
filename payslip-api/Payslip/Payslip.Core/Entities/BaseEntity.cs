namespace Payslip.Core.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id= Guid.NewGuid();
            CreatedAt= DateTime.Now;
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
