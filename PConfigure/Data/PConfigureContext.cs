using Microsoft.EntityFrameworkCore;
using PConfigure.Model;

namespace PConfigure.Data
{
	internal class PConfigureContext : DbContext
	{
		public DbSet<Data_Blockpower> DataBlockpowers { get; set; }
		public DbSet<Data_CPU> DataCPUs { get; set; }
		public DbSet<Data_GPU> DataGPUs { get; set; }
		public DbSet<Data_Memory> DataMemories { get; set; }
		public DbSet<Data_Motherboard> DataMotherboards { get; set; }
		public DbSet<Data_RAM> DataRAMs { get; set; }
		public DbSet<Creator> Creators { get; set; }

		public PConfigureContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=PConfigureDB; Trusted_Connection=True;");
		}
	}
}
