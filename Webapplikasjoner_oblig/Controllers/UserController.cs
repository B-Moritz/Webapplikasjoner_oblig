
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {

        private readonly UserRepository _tradingContext;

        public UserController(UserRepository tradingContext)
        {
            _tradingContext = tradingContext;
        }

        public async Task<bool> Lagre(User innUser)
        {
            return await _tradingContext.Lagre(innUser);
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
