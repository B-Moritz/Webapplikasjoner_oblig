using System;
using System.Reflection;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TradingContext>();

                // må slette og opprette databasen hver gang når den skalinitires (seed'es)
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();


                var user1 = new User { Name = "Okab", LastName = "Mussie", Email = "okab@gmail.com", Password = "8989" };
                var user2 = new User { Name = "Ole", LastName = "Jensen", Email = "ole@gmail.com", Password = "7878" };
                context.Users.Add(user1);
                context.Users.Add(user2);

                context.SaveChanges();

            }
        }
    }
}

