using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Data
{
    // the CatalogContext is a DbContext ( database context )
    // Context is a keyword signifying that you will be writing instructions for your database in this class
    public class CatalogContext: DbContext
    {
        // use a instance of your database as the parameter and pass that into your base class
        // the base class takes the database information to construct your tables and so on
        public CatalogContext(DbContextOptions options) : base(options)
        { 
            
        }
        public DbSet<CatalogType> CatalogTypes {  get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogItem> Catalog { get; set; }

        // model means table, so OnModelCreating  is giving instructions on how to change the tables that you are creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogType>(e =>
            {
                e.Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

                e.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(100);
            });

            modelBuilder.Entity<CatalogBrand>(e =>
            {
                e.Property(b => b.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

                e.Property(b => b.Brand)
                .IsRequired()
                .HasMaxLength(100);
            });

            modelBuilder.Entity<CatalogItem>(e =>
            {
                e.Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

                e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(250);

                e.Property(c => c.Price)
                .IsRequired();

                // this entity has one relationship to the catalog type
                e.HasOne(c => c.CatalogType)
                // in turn, the catalog type has many relationships back to the catalog item
                .WithMany()
                .HasForeignKey(c => c.CatalogTypeId);

                e.HasOne(c => c.CatalogBrand)
                .WithMany()
                .HasForeignKey(c => c.CatalogBrandId);
            });
        }

    }
}
