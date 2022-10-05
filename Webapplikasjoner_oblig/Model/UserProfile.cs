namespace Webapplikasjoner_oblig.Model
{
    public class UserProfile : User
    {
        public Portfolio Portfolio { get; set; }
        public FavoriteList FavoriteList { get; set; }
    }
}
