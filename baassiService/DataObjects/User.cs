using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baassiService.DataObjects
{
    public class User : EntityData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<User> Followers { get; set; }
    }
}
