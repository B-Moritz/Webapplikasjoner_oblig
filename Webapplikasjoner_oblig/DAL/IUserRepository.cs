using System;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface IUserRepository
    {
        // test
        Task<bool> Lagre(User innKunde);
        Task<List<User>> HentAlle();
        Task<bool> Delete(int id);
        Task<User> HentEn(int id);
        Task<bool> Endre(User changeUser);
    }
}

