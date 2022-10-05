namespace Webapplikasjoner_oblig.Model
{
    public class Users
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public decimal FundsAvailable { get; set; }
        public decimal Fundsspent { get; set; }
    }
}
