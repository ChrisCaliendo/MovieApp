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
                //Tag = new Tag() { name = "" };
                var users = new List<User>()
                {
                    new User("Dave", "xxx")
                    {
                        Binges = new List<Binge>()
                        {
                            
                            new Binge {
                                
                                Name = "scary movies",
                                Description = "a buncha scary movies",
                                ShowBinges = new List<ShowBinge>() 
                                {
                                    new ShowBinge{ Show = new Show() {  Title = "Avatar" } },
                                    new ShowBinge{ Show = new Show() {  Title = "Cake Boss"} },
                                    new ShowBinge{ Show = new Show() {  Title = "Impractical Jokers"} }
                                }
                            },
                            new Binge {
                                
                                Name = "funny movies",
                                Description = "a buncha funny movies",
                                ShowBinges = new List<ShowBinge>()
                                {
                                    new ShowBinge{ Show = new Show() { Title = "Neo"} },
                                    new ShowBinge{ Show = new Show() { Title = "Cake Loser"} },
                                    new ShowBinge{ Show = new Show() { Title = "Impractical Fools"} }
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