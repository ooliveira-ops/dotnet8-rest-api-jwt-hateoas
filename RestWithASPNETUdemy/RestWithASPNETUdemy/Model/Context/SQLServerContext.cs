using Microsoft.EntityFrameworkCore;

namespace RestWithASPNETUdemy.Model.Context
{
	public class SQLServerContext : DbContext
	{	//aqui será feita a conexão com o banco
		public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options)
		{
		}

		//aqui será feita a conexão com a tabela - "DbSet" representa uma tabela
		public DbSet<Person> Persons { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<User> Users { get; set; }
	}
}
