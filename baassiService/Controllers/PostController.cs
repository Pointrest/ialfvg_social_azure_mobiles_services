using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using baassiService.DataObjects;
using baassiService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;

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
            List<Post> posts = Query().ToList();
            return Query(); 
        }

        // GET tables/Post
        [Route("table/post/user")]
        public IQueryable<Post> GetAllUserPost()
        {
            // Get the logged in user
            var currentUser = User as ServiceUser;

            return Query().Where(post => post.UserId == currentUser.Id);
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
            // Get the logged in user
            var currentUser = User as ServiceUser;
            var userID = currentUser.Id;

            var userController = new UserController();
            var user = userController.GetUser(userID);

            Post current = null;
         
            item.UserId = currentUser.Id;

            current = await InsertAsync(item);
         

            await sendPushNotification(item, currentUser);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        private async Task sendPushNotification(Post item, ServiceUser currentUser)
        {
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "message", item.Text}
            };
            GooglePushMessage message = new GooglePushMessage(data, TimeSpan.FromHours(1));

            try
            {
                //var result = await Services.Push.SendAsync(message);

                // Use a tag to only send the notification to the logged-in user.
                var result = await Services.Push.SendAsync(message, currentUser.Id);

                Services.Log.Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                Services.Log.Error(ex.Message, null, "Push.SendAsync Error");
            }
        }

        // DELETE tables/Post/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePost(string id)
        {
             return DeleteAsync(id);
        }

    }
}