using System;
using Webapplikasjoner_oblig.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Webapplikasjoner_oblig.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly TradingContext _tradingContext;

        public UserRepository(TradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }

        public async Task<bool> Lagre(User innUser)
        {
            try
            {
                var newUser = new User();
                newUser.Name = innUser.Name;
                newUser.LastName = innUser.LastName;

                _tradingContext.Users.Add(innUser);
                await _tradingContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;

            }
     
        }


     
    }
}
