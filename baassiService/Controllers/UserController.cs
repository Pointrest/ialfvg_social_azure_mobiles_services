using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using baassiService.DataObjects;
using baassiService.Models;
using System;

namespace baassiService.Controllers
{
    public class UserController : TableController<User>
    {
        private baassiContext context = null;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new baassiContext();
            DomainManager = new EntityDomainManager<User>(context, Request, Services);
        }

        // GET tables/User
        public IQueryable<User> GetAllUser()
        {
            return Query(); 
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<IHttpActionResult> PatchUser(string id, Delta<User> patch)
        {
            string updateCommand = "INSERT INTO UserRelations (Followers, Following)  VALUES = ('" +
                patch.GetEntity().Followers +"','" + patch.GetEntity().Following + "');";

            string command = String.Format(updateCommand, ServiceSettingsDictionary.GetSchemaName());
            context.Database.ExecuteSqlCommandAsync(command);
            return null ;
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostUser(User item)
        {
            User current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }

    }
}