using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Entities
{
    public abstract class EntityModel
    {
        public EntityModel()
        {
            Id = new Guid();
        }
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
