namespace Webapplikasjoner_oblig.Model
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FundsAvailable { get; set; }
        public string FundsSpent { get; set; }

        public string Currency { get; set; }
    }
}
