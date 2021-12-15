using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DLL.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        private const string IsDeletedProperty = "IsDeleted";
        
        private static readonly MethodInfo PropertyMethod = typeof(EF)
            .GetMethod(nameof(EF.Property), 
                BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(typeof(bool));
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            // For SoftDelete
            
            // 1. Create the query filter
            var param = Expression.Parameter(type, "it");
            // 2.  EF.Property<bool>(post, "IsDeleted")
            var prop = Expression.Call(PropertyMethod, param, 
                Expression.Constant(IsDeletedProperty));
            // 3 .  EF.Property<bool>(post, "IsDeleted") == false
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop,
                Expression.Constant(false));
            // 4. post => EF.Property<bool>(post, "IsDeleted") == false
            var lambda = Expression.Lambda(condition, param);
        
            return lambda;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // If isdeleted is true dont get the data
                
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType) == true)
                {
                    entity.AddProperty(IsDeletedProperty, typeof(bool));
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }
            
            // many to many relation mapping
            modelBuilder.Entity<CourseStudent>()
                .HasKey(bc => new { bc.CourseId, bc.StudentId });  
            modelBuilder.Entity<CourseStudent>()
                .HasOne(bc => bc.Course)
                .WithMany(b => b.CourseStudents)
                .HasForeignKey(bc => bc.CourseId);  
            modelBuilder.Entity<CourseStudent>()
                .HasOne(bc => bc.Student)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(bc => bc.StudentId);
            
            
            base.OnModelCreating(modelBuilder);
        }
        
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSavingData();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        
        private  void OnBeforeSavingData()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached && e.State != EntityState.Unchanged);
        
            foreach (var entry in entries)
            {
                if (entry.Entity is  ITrackable trackable)
                {
                    switch(entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = DateTimeOffset.Now;
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Modified:
                            trackable.LastUpdatedAt = DateTimeOffset.Now;
                            break;
                        case EntityState.Deleted:
                            //when something is deleted set the value to true
                            entry.Property(IsDeletedProperty).CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
            }
        }
        
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSavingData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
    }
}
 