using SignalR.Api.Model.Interface;

namespace SignalR.Api.Model.Base
{
    public abstract class Lookup : BaseEntity, IFriendlyNamedObject, ILookup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50), Required]
        public string Name { get; set; }

        [StringLength(50), Required]
        public string FriendlyName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
