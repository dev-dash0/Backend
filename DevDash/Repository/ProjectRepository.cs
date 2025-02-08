using DevDash.model;
using DevDash.Repository.IRepository;

namespace DevDash.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private AppDbContext _db;
        public ProjectRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Project> UpdateAsync(Project project)
        {
            var projectFromDb = _db.Projects.FirstOrDefault(u=>u.Id == project.Id);
            if (projectFromDb != null)
            {
                projectFromDb.Name = project.Name;
                projectFromDb.Description = project.Description;
                projectFromDb.StartDate = project.StartDate;
                projectFromDb.EndDate = project.EndDate;
                projectFromDb.Priority = project.Priority;
                projectFromDb.Status = project.Status;
            }
            await _db.SaveChangesAsync();
            return project;
        }
    }
}
