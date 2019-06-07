using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWebApplication.Repository
{
    public class UserRepo
    {
        private RepshoDBE db;

        public UserRepo()
        {
            db = new RepshoDBE();
        }
        ~UserRepo()
        {
            db.Dispose();
            db = null;
        }

        public UserMaster GetUser(int id)
        {
            var user = db.UserMasters.Where(u => u.UserID == id).FirstOrDefault();
            return user;
        }

    }
}