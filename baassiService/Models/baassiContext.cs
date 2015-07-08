using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using baassiService.DataObjects;

namespace baassiService.Models
{
    public class baassiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.
        private const string connectionStringName = "Name=MS_TableConnectionString";

        public baassiContext() : base(connectionStringName)
        {
        } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = ServiceSettingsDictionary.GetSchemaName();
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }

            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            modelBuilder.Entity<User>()
                   .HasMany<User>(u => u.Followers)
                   .WithMany(c => c.Following)
                   .Map(cs =>
                   {
                       cs.MapLeftKey("Followers");
                       cs.MapRightKey("Following");
                       cs.ToTable("UserRelations");
                   });
        }

        public System.Data.Entity.DbSet<baassiService.DataObjects.Post> Posts { get; set; }
        public System.Data.Entity.DbSet<baassiService.DataObjects.User> Users { get; set; }
    }
}
