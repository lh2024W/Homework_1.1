using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Homework_1._1
{
    public class Train
    {
        public int Id { get; set; }
        public string Number { get; set; } // номер поезда
        public string Route { get; set; } // маршрут Киев-Одесса
        public string Arrival { get; set; } //Прибытие (время)
        public string Departure { get; set; } // Отправление (время)
                
     }

    class ApplicationContext : DbContext
    {
        public DbSet<Train> Trains { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }
    }

    public class DatabaseTrain
    {
        private DbContextOptions <ApplicationContext> GetConnectionOptions ()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }

        public void EnsurePopulate()
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                Train train = new Train
                {
                    Number = "1209",
                    Route = "Одесса-Киев",
                    Arrival = "08.15",
                    Departure = "16.30"
                };
                db.Add(train);
                db.SaveChanges();
            }
        }
        public void AddTrain(Train train)
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
                db.Trains.Add(train);
                db.SaveChanges();
            }
        }

        public void Update(Train train)
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
                db.Trains.Update(train);
                db.SaveChanges();
            }
        }

        public Train GetTrainById(int id)
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
                return db.Trains.FirstOrDefault(e => e.Id == id);
            }
        }

        public void RemoveTrain(Train train)
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
                db.Trains.Remove(train);
                db.SaveChanges();
            }
        }
        
    }
    public class Program
    {
        static void Main()
        {
            DatabaseTrain dbTrain = new DatabaseTrain();
            dbTrain.EnsurePopulate();
            
            Train train = new Train
            {
                Number = "5698",
                Route = "Одесса-Запорожье",
                Arrival = "12.00",
                Departure = "20.25"
            };
            dbTrain.AddTrain(train);
            dbTrain.Update(train);
            
            var t = dbTrain.GetTrainById(1);
            if (t != null)
            {
                t.Arrival = "15.00";
                t.Departure = "22.10";
                dbTrain.Update(t);
            }

            var tr = dbTrain.GetTrainById(1);
            if (tr != null)
            {
                dbTrain.RemoveTrain(tr);
            }
            
        }
    }
}


    

    

