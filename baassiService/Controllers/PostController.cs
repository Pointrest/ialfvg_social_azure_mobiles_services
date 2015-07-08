using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using baassiService.DataObjects;
using baassiService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace baassiService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class PostController : TableController<Post>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            baassiContext context = new baassiContext();
            DomainManager = new EntityDomainManager<Post>(context, Request, Services);
        }

        // GET tables/Post
        public IQueryable<Post> GetAllPost()
        {
            return Query(); 
        }

        // GET tables/Post/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Post> GetPost(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Post/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Post> PatchPost(string id, Delta<Post> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Post
        public async Task<IHttpActionResult> PostPost(Post item)
        {
            Post current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Post/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePost(string id)
        {
             return DeleteAsync(id);
        }

    }
}