using DevDash.model;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // DbSets for entities
    public DbSet<User> Users { get; set; }  
    public DbSet<Tenant> Tenants { get; set; } 
    public DbSet<Project> Projects { get; set; }  
    public DbSet<UserTenant> UserTenants { get; set; }  
    public DbSet<Team> Teams { get; set; }  
    public DbSet<TeamMember> TeamMembers { get; set; } 
    public DbSet<Sprint> Sprints { get; set; }  
    public DbSet<Issue> Issues { get; set; }  
    public DbSet<IssueAssignedUser> IssueAssignedUsers { get; set; }  
    public DbSet<Comment> Comments { get; set; }  
    //public DbSet<StandardEntity> StandardEntities { get; set; }  
    public DbSet<PersonalTask> PersonalTasks { get; set; }  
    public DbSet<Notification> Notifications { get; set; }  
    public DbSet<PrivateNote> PrivateNotes { get; set; }  
    public DbSet<Integration> Integrations { get; set; }  

    // Configure the connection string (replace with your actual string)
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DevDash;Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True");
    }

    // Configure the models and relationships using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User-Tenant Many-to-Many Relationship (UserTenant entity)
        // A user can belong to many tenants, and a tenant can have many users
        modelBuilder.Entity<UserTenant>()
            .HasKey(ut => new { ut.UserId, ut.TenantId });  // Composite key for UserTenant table

        // Relationship: User -> UserTenants (1-to-many)
        modelBuilder.Entity<UserTenant>()
            .HasOne(ut => ut.User)  
            .WithMany(u => u.UserTenants)  // A user can have many UserTenants
            .HasForeignKey(ut => ut.UserId)  // Foreign key for UserId in UserTenant
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a user is deleted, their UserTenants are deleted

        // Relationship: Tenant -> UserTenants (1-to-many)
        modelBuilder.Entity<UserTenant>()
            .HasOne(ut => ut.Tenant)  
            .WithMany(t => t.UserTenants)  // A tenant can have many UserTenants
            .HasForeignKey(ut => ut.TenantId)  // Foreign key for TenantId in UserTenant
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a tenant is deleted, their UserTenants are deleted

        // TeamMember Many-to-Many Relationship
        // A team can have many members, and a user can be a member of many teams
        modelBuilder.Entity<TeamMember>()
            .HasKey(tm => new { tm.TeamId, tm.MemberId });  // Composite key for TeamMember table

        // Relationship: User -> TeamMembers (1-to-many)
        modelBuilder.Entity<TeamMember>()
            .HasOne(tm => tm.Member)  
            .WithMany(u => u.TeamMembers)  // A user can be part of many teams
            .HasForeignKey(tm => tm.MemberId);  // Foreign key for MemberId in TeamMember

        // Relationship: Team -> TeamMembers (1-to-many)
        modelBuilder.Entity<TeamMember>()
            .HasOne(tm => tm.Team)  
            .WithMany(t => t.TeamMembers)  // A team can have many members
            .HasForeignKey(tm => tm.TeamId)  // Foreign key for TeamId in TeamMember
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a team is deleted, its members are deleted

        // IssueAssignedUser Many-to-Many Relationship
        // A user can be assigned to many issues, and an issue can have many assigned users
        modelBuilder.Entity<IssueAssignedUser>()
            .HasKey(iau => new { iau.IssueId, iau.UserId });  // Composite key for IssueAssignedUser table

        // Relationship: User -> IssueAssignedUsers (1-to-many)
        modelBuilder.Entity<IssueAssignedUser>()
            .HasOne(iau => iau.User)  
            .WithMany(u => u.IssueAssignedUsers)  // A user can be assigned to many issues
            .HasForeignKey(iau => iau.UserId)  // Foreign key for UserId in IssueAssignedUser
            .OnDelete(DeleteBehavior.SetNull);  // When an issue is deleted, the user assignment is set to null

        // Relationship: Issue -> IssueAssignedUsers (1-to-many)
        modelBuilder.Entity<IssueAssignedUser>()
            .HasOne(iau => iau.Issue)  
            .WithMany(i => i.IssueAssignedUsers)  // An issue can have many assigned users
            .HasForeignKey(iau => iau.IssueId)  // Foreign key for IssueId in IssueAssignedUser
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when an issue is deleted, its assignments are deleted

        // Comment - Issue Relationship
        // A comment is made by a user and is associated with an issue
        // Relationship: User -> Comments (1-to-many)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.CreatedByUser)  
            .WithMany(u => u.Comments)  // A user can make many comments
            .HasForeignKey(c => c.CreatedBy)  // Foreign key for CreatedBy in Comment
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a user is deleted, their comments are deleted

        // PersonalTask - User Relationship
        // A personal task belongs to a user
        // Relationship: User -> PersonalTasks (1-to-many)
        modelBuilder.Entity<PersonalTask>()
            .HasOne(pt => pt.User)  
            .WithMany(u => u.PersonalTasks)  // A user can have many personal tasks
            .HasForeignKey(pt => pt.UserId)  // Foreign key for UserId in PersonalTask
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a user is deleted, their personal tasks are deleted

        // PrivateNote - User Relationship
        // A private note belongs to a user
        // Relationship: User -> PrivateNotes (1-to-many)
        modelBuilder.Entity<PrivateNote>()
            .HasOne(pn => pn.User)  
            .WithMany(u => u.PrivateNotes)  // A user can have many private notes
            .HasForeignKey(pn => pn.UserId)  // Foreign key for UserId in PrivateNote
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a user is deleted, their private notes are deleted

        // Project - Tenant Relationship
        // A project belongs to a tenant
        // Relationship: Tenant -> Projects (1-to-many)
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Tenant)  
            .WithMany(t => t.Projects)  // A tenant can have many projects
            .HasForeignKey(p => p.TenantId)  // Foreign key for TenantId in Project
            .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: when a tenant is deleted, its projects are deleted

        // ProjectManager - Project Relationship
        // A project can have a project manager (user)
        // Relationship: User -> Projects (1-to-many)
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Manager)
            .WithMany(u => u.ManagedProjects)  // A user can manage many projects
            .HasForeignKey(p => p.ProjectManagerId)  // Foreign key for ProjectManagerId in Project
            .OnDelete(DeleteBehavior.SetNull);  // Set null: when a project manager is deleted, the manager field is set to null

        // Team - Supervisor Relationship
        // A team has a supervisor (user)
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Supervisor)  // Relationship: User -> Teams (1-to-many)
            .WithMany(u => u.SupervisedTeams)  // A user can supervise many teams
            .HasForeignKey(t => t.SupervisorId)  // Foreign key for SupervisorId in Team
            .OnDelete(DeleteBehavior.SetNull);  // Set null: when a supervisor is deleted, the supervisor field is set to null

        // Configuring indexes and uniqueness
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)  // Ensure username is unique
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)  // Ensure email is unique
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)  // Ensure phone number is unique
            .IsUnique();

        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.TenantCode)  // Ensure tenant code is unique
            .IsUnique();

        modelBuilder.Entity<Integration>()
            .HasIndex(i => i.Name)  // Ensure integration name is unique
            .IsUnique();
    }
}
