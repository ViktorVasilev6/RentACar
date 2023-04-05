using System;

namespace RentACar.Models
{
    public class Query
    {
        public int Id { get; set; }
        public string Renter { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int CarId { get; set; }
    }
}
