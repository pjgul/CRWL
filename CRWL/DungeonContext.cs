namespace CRWL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DungeonContext : DbContext
    {
        // Your context has been configured to use a 'DungeonContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CRWL.DungeonContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DungeonContext' 
        // connection string in the application configuration file.
        public DungeonContext()
            : base("name=DungeonContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual dbset<myentity> myentities { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Monster> Monsters { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}