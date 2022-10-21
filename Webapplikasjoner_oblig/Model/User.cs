namespace Webapplikasjoner_oblig.Model
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public decimal FundsAvailable { get; set; }
        public decimal FundsSpent { get; set; }

        public string Currency { get; set; }
    }
}
