using Application.Common.Interfaces;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _datetime;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly CurrentUser _user;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IDateTimeService datetime,
            IDomainEventDispatcher domainEventDispatcher,
            ICurrentUserService currentUserService) : base(options)
        {
            //agregamos para poder seguir los cambios y que Entity se de cuenta cuando hace un SaveAsync
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this._datetime = datetime;
            _domainEventDispatcher = domainEventDispatcher;
            _user = currentUserService.User;
        }

        
        public DbSet<City> Cities { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        //Sobrescribimos SaveAsync
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //Cada vez que guardamos o modificamos le decimos que guarde la fecha
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _user.Id;
                        entry.Entity.CreatedDate = _datetime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _user.Id;
                        entry.Entity.ModifiedDate = _datetime.Now;
                        break;
                    case EntityState.Deleted:

                        // Si la entidad tiene la propiedad Address, la dejamos sin cambios.
                        if (entry.Metadata.FindNavigation("Address") != null)
                        {
                            var addressEntry = entry.Reference("Address").TargetEntry as Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry;
                            if (addressEntry != null)
                            {
                                addressEntry.State = EntityState.Unchanged;
                            }
                        }

                        // Si la entidad tiene la propiedad Phone, la dejamos sin cambios.
                        if (entry.Metadata.FindNavigation("Phone") != null)
                        {
                            var phoneEntry = entry.Reference("Phone").TargetEntry as Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry;
                            if (phoneEntry != null)
                            {
                                phoneEntry.State = EntityState.Unchanged;
                            }
                        }
                        entry.State = EntityState.Modified;

                        entry.Entity.DeletedBy = _user.Id;
                        entry.Entity.DeletedDate = _datetime.Now;
                        break;
                }
            }
            //return base.SaveChangesAsync();

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Ignora lo eventos si el dispatcher no tiene nada.
            if (_domainEventDispatcher == null) return result;

            // Ejecuta los eventos solo si el Save fue exitoso.
            //var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            //    .Select(e => e.Entity)
            //    .Where(e => e.DomainEvents.Any())
            //    .ToArray();
            var entitiesWithEvents = ChangeTracker
                .Entries<IEntityWithEvents>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            await _domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Aca le decimos que se ejecute la configuracion de cada entidad para la migracion
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}