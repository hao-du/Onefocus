namespace Onefocus.Common.Abstractions.Domain
{
    public class EntityBase
    {
        public Guid Id { get; protected set; }
        public string? Description { get; protected set; }
        public bool IsActive { get; protected set; }
        public DateTimeOffset? CreatedOn { get; protected set; }
        public DateTimeOffset? UpdatedOn { get; protected set; }
        public Guid? CreatedBy { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }
    }
}
