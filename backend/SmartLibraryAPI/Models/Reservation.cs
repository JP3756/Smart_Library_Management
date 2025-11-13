namespace SmartLibraryAPI.Models
{
    public enum ReservationStatus
    {
        Pending,
        Ready,
        Fulfilled,
        Cancelled,
        Expired
    }

    /// <summary>
    /// Reservation entity - allows users to reserve books
    /// </summary>
    public class Reservation
    {
        public int Id { get; set; }
        
        // Foreign keys
        public int UserId { get; set; }
        public int BookId { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;

        public DateTime ReservationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? FulfilledDate { get; set; }
        public ReservationStatus Status { get; set; }

        // Business logic
        public void MarkAsReady()
        {
            Status = ReservationStatus.Ready;
            ExpiryDate = DateTime.UtcNow.AddDays(3); // 3 days to pick up
        }

        public void Fulfill()
        {
            Status = ReservationStatus.Fulfilled;
            FulfilledDate = DateTime.UtcNow;
        }

        public void Cancel()
        {
            Status = ReservationStatus.Cancelled;
        }

        public void CheckExpiry()
        {
            if (Status == ReservationStatus.Ready && 
                ExpiryDate.HasValue && 
                DateTime.UtcNow > ExpiryDate.Value)
            {
                Status = ReservationStatus.Expired;
            }
        }

        public Reservation()
        {
            ReservationDate = DateTime.UtcNow;
            Status = ReservationStatus.Pending;
        }
    }
}
