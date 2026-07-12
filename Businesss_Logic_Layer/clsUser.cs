using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Businesss_Logic_Layer;
using DataAccesesLayer;

namespace Businesss_Logic_Layer
{
    public class clsUser
    {
        public enum enMode { enAdd, enUpdate }
        private enMode _mode;

        public int UserId { get; private set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public enUserRole Role { get; set; }


        public clsUser()
        {
            UserId = -1;
            PersonId = -1;
            UserName = "";
            Password = "";
            IsActive = true;
           Role = enUserRole.Employee;
            _mode = enMode.enAdd;
        }


        private clsUser(int userId, int personId, string userName,
            string password, bool isActive, enUserRole role)
        {
            UserId = userId;
            PersonId = personId;
            UserName = userName;
            Password = password;
            IsActive = isActive;
           Role = role;
            _mode = enMode.enUpdate;
        }

       
        public static clsUser FindByUsernameAndPassword(
            string username, string password)
        {
            int userId = -1, personId = -1;
            bool isActive = false;
            byte roleId = 0;

            bool found = clsUsersDataAccesses.FindUser(
                username, password,
                ref userId, ref personId,
                ref isActive, ref roleId);

            if (!found) return null;

            return new clsUser(userId, personId, username,
                password, isActive, (enUserRole)roleId);
        }

       
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(UserName)) return false;
            if (string.IsNullOrWhiteSpace(Password)) return false;
            if (PersonId <= 0) return false;
            return true;
        }

      
        private bool Add()
        {
            int newId = clsUsersDataAccesses.AddUser(
                UserName, Password, IsActive, (byte)Role);
            if (newId == -1) return false;
            UserId = newId;
            return true;
        }

        private bool Update()
        {
            return clsUsersDataAccesses.UpdateUser(
                UserId, UserName, Password, IsActive, (byte)Role);
        }

        public bool Save()
        {
            if (!Validate()) return false;
            switch (_mode)
            {
                case enMode.enAdd: return Add();
                case enMode.enUpdate: return Update();
                default: return false;
            }
        }

     
        public static bool Delete(int userId)
            => clsUsersDataAccesses.DeleteUser(userId);
    }
}