using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure._Identity
{
	public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUsers>
	{
		public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUsers>()
				.HasOne(U => U.Address)
				.WithOne(A => A.User)
				.HasForeignKey<Address>(A => A.ApplicationUserId);
		}

	}
}
