using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class UsersEntity
    {
        public int UsersID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string loginuser { get; set; }

        public string password { get; set; }
        public string Role { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Int64 UpdatedBy { get; set; }
    
    }
}
