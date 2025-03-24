using Microsoft.EntityFrameworkCore;
using NZWalkRevise.Models.DomainModels;

namespace NZWalkRevise.Database
{
    public class NZWalkDbContext : DbContext
    {
        public NZWalkDbContext(DbContextOptions<NZWalkDbContext> options) : base(options)
        {

        }

        public DbSet<Walk> Walks { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Added by Raghvendra to Seed the data to Difficulty Table.
            var difficultyList = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id= Guid.Parse("aba0e9fa-6146-40b4-b813-20a5f9a4872d"),
                    Name="Easy"
                },
                new Difficulty()
                {
                    Id=Guid.Parse("d62a659f-0b5b-4e2a-ab4b-bd636c185a46"),
                    Name="Medium"
                },
                new Difficulty()
                {
                    Id=Guid.Parse("488d8c58-aa04-4023-b10d-db3d85b54b97"),
                    Name="Hard"
                },
            };

            modelBuilder.Entity<Difficulty>().HasData(difficultyList);

            var regionList = new List<Region>()
            {
                new Region()
                {
                    Id=Guid.Parse("3ce23c73-b1f3-48a4-9102-9dc079f29d4e"),
                    Name="Northland",
                    Code="NLD",
                    Description="Northland is a region in the northernmost part of New Zealand. Northland is the warmest region in New Zealand.",
                    RegionImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png"
                },
                new Region()
                {
                    Id=Guid.Parse("c4a251c6-4eff-4c51-977c-719542149fa7"),
                    Name="Auckland",
                    Code="AKL",
                    Description="Auckland is a metropolitan city in the North Island of New Zealand. The most populous urban area in the country.",
                    RegionImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png"
                },
                new Region()
                {
                    Id=Guid.Parse("18608543-2350-4e91-b553-41c62c376dd8"),
                    Name="Waikato",
                    Code="WKT",
                    Description="Waikato is a region in the upper North Island of New Zealand. Waikato is known for its dairy farming and horse breeding.",
                    RegionImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png"
                },
                new Region()
                {
                    Id=Guid.Parse("00f69924-2bac-4589-a7ea-956a091259fb"),
                    Name="Bay of Plenty",
                    Code="BOP",
                    Description="The Bay of Plenty is a region in the North Island of New Zealand. The region takes its name from the large bay at its heart.",
                    RegionImageUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png"
                }
            };
            modelBuilder.Entity<Region>().HasData(regionList);
        }
    }
}
