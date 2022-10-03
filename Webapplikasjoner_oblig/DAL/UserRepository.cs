using System;
using Webapplikasjoner_oblig.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Webapplikasjoner_oblig.DAL
{
    public class UserRepository
    {
        private readonly TradingContext _tradingContext;

        public UserRepository(TradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }

        /*
        public async Task<bool> Lagre(User innKunde)
        {
            try
            {
                var nyUserRad = new Users();
                nyUserRad.Name = innKunde.Name;
                nyUserRad.LastName = innKunde.LastName;
                nyUserRad.Email = innKunde.Email;
                nyUserRad.Password = innKunde.Password;
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> HentAlle()
        {
            try
            {
                List<User> allUsers = await _tradingContext.Users.Select(U => new User
                {
                    Id = U.Id,
                    Name = U.Name,
                    LastName = U.LastName,
                    Email = U.Email,
                    Password = U.Password
                }).ToListAsync();
                return allUsers;
            }
            catch
            {
                return null;

            }

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                Users enDBUser = await _tradingContext.Users.FindAsync(id);
                _tradingContext.Users.Remove(enDBUser);
                await _tradingContext.SaveChangesAsync();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<User> HentEn(int id)
        {
            try
            {
                Users enUser = await _tradingContext.Users.FindAsync(id);
                var hentetKunde = new User()
                {
                    Id = enUser.Id,
                    Name = enUser.Name,
                    LastName = enUser.LastName,
                    Email = enUser.Email,
                    Password = enUser.Password
                };
                return hentetKunde;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Endre(User endreKunde)
        {
            var endreObjekt = await _tradingContext.Users.FindAsync(endreKunde.Id);
            if (endreObjekt.Email)
        }
        */


        // .....
       
        public async Task<bool> Lagre(User innUser)
        {
            try
            {
                _tradingContext.Users.Add(innUser);
                await _tradingContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<User>> HentAlle()
        {
            try
            {
                List<User> allUsers = await _tradingContext.Users.ToListAsync(); // hent hele tabellen
                return allUsers;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                User enUser = await _tradingContext.Users.FindAsync(id);
                _tradingContext.Users.Remove(enUser);
                await _tradingContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /*
        public async Task<User> HentEn(int id)
        {
            try
            {
                User enUser = await _tradingContext.Users.FindAsync(id);
                return enUser;
            }
            catch
            {
                return null;
            }
        }*/

        public async Task<User> HentEn(int id)
        {
            try
            {
                User enUser = await _tradingContext.Users.FindAsync(id);
                var hentetKunde = new User()
                {
                    Id = enUser.Id,
                    Name = enUser.Name,
                    LastName = enUser.LastName,
                    Email = enUser.Email,
                    Password = enUser.Password
                };
                return hentetKunde;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Endre(User changeUser)
        {
            try
            {
                User enUser = await _tradingContext.Users.FindAsync(changeUser.Id);
                enUser.Name = changeUser.Name;
                enUser.LastName = changeUser.LastName;
                enUser.Email = changeUser.Email;
                enUser.Password = changeUser.Password;
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
