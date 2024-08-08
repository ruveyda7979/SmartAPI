using DBSmartAPIManager.DAL.Context;
using DBSmartAPIManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL.Services
{
    public class UserService:Repository<User>
    {
        public UserService(DBSmartAPIManagerContext context) : base(context) 
        {
            
        }

        //Kullanıcı doğrulama metodu
        public async Task<User> ValidateUserAsync(string email, string password)
        {
            // Veritabanında email'e sahip kullanıcıyı bul
            var user = await SelectAsync(u => u.Email == email);

            if(user!= null && user.Password==password)
            {
                //Kullanıcı bulundu ve şifre doğru
                return user;
            }

            //Kullanıcı bulunamadı veya şifre yanlış 
            return null;
        }

        //Yeni kullanıcı kaydetme  metodu
        public async Task<bool> RegisterUserAsync(User newUser)
        {
            //Aynı email ile kullanıcı var mı kontrol et
            var existingUser = await SelectAsync(u => u.Email == newUser.Email);

            if(existingUser == null)
            {
                // Yeni kullanıcıyı veritabanına ekle 
                await SaveAsync(newUser);
                return true;
            }

            //Aynı email ile kullanıcı zaten var 
            return false;
        }
    }
}
