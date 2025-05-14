using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDotnet.Config
{
    public class Session
    {
        public static void SetSession(int UserId, string Username, string Userrole)
        {
            UserID = UserId;
            UserName = Username;
            UserRole = Userrole;
        }

        public static void ClearSession()
        {
            UserID = -1;
            UserName = string.Empty;
            UserRole = string.Empty;
        }
        public static int UserID { get; set; } = -1;
        public static string UserName { get; set; } = string.Empty;
        public static string UserRole { get; set; } = string.Empty;
    }
}
