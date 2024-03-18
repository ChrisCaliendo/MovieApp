using MovieApp.Data;
using MovieApp.Models;
using System.Diagnostics.Metrics;

namespace MovieApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {
                var users = new List<User>()
                {
                    new User("Dave", "xxx")
                    {
                        binges = new List<Binge>()
                        {
                            new Binge {
                                
                                name = "scary movies",
                                description = "a buncha scary movies",
                                showBinges = new List<ShowBinge>() 
                                {
                                    new ShowBinge{ show = new Show() {  title = "Avatar"} },
                                    new ShowBinge{ show = new Show() {  title = "Cake Boss"} },
                                    new ShowBinge{ show = new Show() {  title = "Impractical Jokers"} }
                                }
                            },
                            new Binge {
                                
                                name = "funny movies",
                                description = "a buncha funny movies",
                                showBinges = new List<ShowBinge>()
                                {
                                    new ShowBinge{ show = new Show() { title = "Neo"} },
                                    new ShowBinge{ show = new Show() { title = "Cake Loser"} },
                                    new ShowBinge{ show = new Show() { title = "Impractical Fools"} }
                                }
                            }
                        },

                    },
                    
                };
                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            }
        }
    }
}