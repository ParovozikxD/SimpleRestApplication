using Microsoft.EntityFrameworkCore;
using System;

namespace SimpleRestApplication.Entities
{
    public class DataBase
    {
        
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                using (UserContext userContext = new UserContext())
                {             
                    return await userContext.Users.ToListAsync(); 
                }
            }
            catch
            {
                return null;
            }
        }


        public async Task<bool> AddUser(User user)
        {
            bool success = false;
            try
            {
                using (UserContext userContext = new UserContext())
                {
                    await userContext.AddAsync(user);
                    await userContext.SaveChangesAsync();

                    success = true;

                    return success;
                }
            }
            catch
            {
                return success;
            }
        }

        public async Task<bool> RemoveUser(string userID)
        {
            bool success = false;
            try
            {
                using (UserContext userContext = new UserContext())
                {
                    var user = await userContext.Users.FindAsync(userID);

                    if (user != null)
                    {
                        userContext.Users.Remove(user);
                        await userContext.SaveChangesAsync();
                        success = true;

                        return success;
                    }
                    else
                    {
                        return success;
                    }
                
                }
            }
            catch
            {
                return success;
            }
        }


        public async Task<bool> UpdateUser(string userID, string newName, int newAge)
        {
            bool success = false;

            try
            {
                using(UserContext userContext = new UserContext())
                {
                    var foundUser = await userContext.Users.FindAsync(userID);

                    if(foundUser != null)
                    {
                        foundUser.Name = newName;
                        foundUser.Age = newAge;

                        await userContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ArgumentNullException("Cant found User");
                    }
                  
                }

                success = true;
                return success;
            }
            catch
            {

                return success;
            }


        }
    }
}
