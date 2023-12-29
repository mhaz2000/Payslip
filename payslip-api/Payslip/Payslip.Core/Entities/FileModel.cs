namespace Payslip.Core.Entities
{
    public class FileModel : BaseEntity
    {
        public FileModel(string name, Guid id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }

    }
}
