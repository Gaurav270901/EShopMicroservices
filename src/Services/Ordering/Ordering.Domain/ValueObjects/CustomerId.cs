

namespace Ordering.Domain.ValueObjects
{
    //of method : 
    //factory method to create instance of CustomerId value object
    //ensures that the value object is always created in a valid state
    //by validating the input before creating the instance
    public record CustomerId
    {
        public Guid Value { get; }

        private CustomerId(Guid value)
        {
            Value = value;
        }

        public static CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            if(value == Guid.Empty)
            {
                throw new DomainException("CustomerId value cannot be an empty GUID.");
            }
            return new CustomerId(value);
        }
    }


}
