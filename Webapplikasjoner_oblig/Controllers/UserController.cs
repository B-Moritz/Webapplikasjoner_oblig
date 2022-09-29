
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _tradingContext;
        public UserController(IUserRepository tradingContext)
        {
            _tradingContext = tradingContext;
        }


        // ...
        public async Task<bool> Lagre(User innKunde)
        {
            return await _tradingContext.Lagre(innKunde);
        }
        public async Task<List<User>> HentAlle()
        {
            return await _tradingContext.HentAlle();
        }
        public async Task<bool> Delete(int id)
        {
            return await _tradingContext.Delete(id);
        }
        public async Task<User> HentEn(int id)
        {
            return await _tradingContext.HentEn(id);
        }
        public async Task<bool> Endre(User changeUser)
        {
            return await _tradingContext.Endre(changeUser);
        }
    }
}
