using System.Text;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface ITradingRepository
    {
        Task<bool> Lagre(Trading innTrading);
        Task<List<Trading>> HentAlleTrading();
        Task<Trading> HentEnTrading(int id);
    }
}
