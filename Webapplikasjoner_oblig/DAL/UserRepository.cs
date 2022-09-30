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



        // .....
        /*
        public async Task<bool> Lagre(User innKunde)
        {
            try
            {
                _tradingContext.Users.Add(innKunde);
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
                List<User> alleKundene = await _tradingContext.Users.ToList(); // hent hele tabellen
                return alleKundene;
            }
            catch
            {
                return null;
            }
        }

        public bool Slett(int id)
        {
            try
            {
                User enKunde = _tradingContext.Users.Find(id);
                _tradingContext.Users.Remove(enKunde);
                _tradingContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User HentEn(int id)
        {
            try
            {
                User enKunde = _tradingContext.Users.Find(id);
                return enKunde;
            }
            catch
            {
                return null;
            }
        }

        public bool Endre(User endreKunde)
        {
            try
            {
                User enKunde = _tradingContext.Users.Find(endreKunde.Id);
                enKunde.Name = endreKunde.Name;
                enKunde.LastName = endreKunde.LastName;
                _tradingContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        */





        // .....
    }
}
