using Microsoft.EntityFrameworkCore;
using StudentBloggAPI.Data;
using StudentBloggAPI.Models.Entities;
using StudentBloggAPI.Repository.Interfaces;
using StudentBloggAPI.Services;

namespace StudentBloggAPI.Repository
{
    public class PostRepository : IPostRepository
    {

        private readonly StudentBloggDbContext _dbContext;

        public PostRepository(StudentBloggDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Post?> AddPostAsync(Post post)
        {
            await _dbContext.Post.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post?> DeletePostByIdAsync(int id)
        {
			var user = await _dbContext.Post.FirstOrDefaultAsync(x => x.Id == id);

			if (user == null)
				return null;

			await _dbContext.Users.Where(x => x.Id == id)
				.ExecuteDeleteAsync();

			_dbContext.SaveChanges();

			return user;
		}

        public async Task<Post?> GetPostByIdAsync(int id)
        {
			var user = await _dbContext.Post.FirstOrDefaultAsync(x => x.Id == id);

			return user;
		}

        public async Task<ICollection<Post>> GetPostsAsync()
        {
			return await _dbContext.Post.ToListAsync();
		}

		public async Task<Post?> UpdatePostAsync(int id, Post user)
        {
			var existingUser = await _dbContext.Post.FirstOrDefaultAsync(x => x.Id == id);

			if (existingUser == null)
			{
				return null; // Brukeren med den angitte ID-en ble ikke funnet.
			}

			// Gjør oppdateringer på den eksisterende brukeren med de nye opplysningene.
			existingUser.Title = user.Title;
			existingUser.Content = user.Content;
			existingUser.Updated = user.Updated;

			// Lagre endringene i databasen.
			await _dbContext.SaveChangesAsync();

			return existingUser;
		}
	}
}
