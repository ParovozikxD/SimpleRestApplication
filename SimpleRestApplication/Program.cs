using SimpleRestApplication.Entities;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
//TestCommit
DataBase dataBase = new DataBase();

app.Run(async (context) =>
{
    var regexGuiDPattern = @"\w{8}-\w{4}-\w{4}-\w{4}-\w{12}";


    if (context.Request.Path == "/getAllUsers" && context.Request.Method == "GET")
    {
        var userList = await dataBase.GetAllUsers();

        if (userList != null)
        {
            await context.Response.WriteAsJsonAsync(userList);
        }
        else
        {
            context.Response.StatusCode = 400;

            var errorDetails = new
            {
                message = "An error occurred while processing the request.",
                exception = "Cannot Access to the DataBase"
            };

            await context.Response.WriteAsJsonAsync(errorDetails);
        }

    }
    else if (context.Request.Path == "/addUser" && context.Request.Method == "POST")
    {
        try
        {
            var userData = await context.Request.ReadFromJsonAsync<User>();

            User user = new User(userData.Name, userData.Age);

            if (user != null)
            {
                var success = await dataBase.AddUser(user);

                if (!success)
                {
                    throw new InvalidOperationException("Failed to add user in DataBase");
                }
            }
            else
            {
                throw new ArgumentNullException("User object is null");
            }

            await context.Response.WriteAsJsonAsync(user);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 400;

            var errorDetails = new
            {
                message = "An error occurred while processing the request.",
                exception = ex.ToString()
            };

            await context.Response.WriteAsJsonAsync(errorDetails);
        }
    }
    else if (context.Request.Method == "DELETE" && Regex.IsMatch(context.Request.Path, $"^/removeUser/{regexGuiDPattern}$"))
    {
        try
        {
            var userID = Regex.Match(context.Request.Path, $"{regexGuiDPattern}$").Value;

            if (userID != null)
            {
                var success = await dataBase.RemoveUser(userID);

                if (!success)
                {
                    throw new InvalidOperationException("Cannot delete the User instance");
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 400;

            var errorDetails = new
            {
                message = "An error occurred while processing the request.",
                exception = ex.ToString()
            };

            await context.Response.WriteAsJsonAsync(errorDetails);
        }

    }
    else if (context.Request.Method == "PUT" && Regex.IsMatch(context.Request.Path, $"^/updateUser/{regexGuiDPattern}$"))
    {
        try
        {
            var updatedUser = await context.Request.ReadFromJsonAsync<User>();

            var userID = Regex.Match(context.Request.Path, $"{regexGuiDPattern}$").Value;


            if(updatedUser != null && !string.IsNullOrEmpty(userID))
            {

                var success = await dataBase.UpdateUser(userID, updatedUser.Name, updatedUser.Age);

                if (!success)
                {
                    throw new InvalidOperationException("Cannot update the User instance");
                }
                
                await context.Response.WriteAsJsonAsync(updatedUser);
            }
            else
            {
                throw new ArgumentException("Request user is null");
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 404;

            var errorDetails = new
            {
                message = "An error occurred while processing the request.",
                exception = ex.ToString()
            };

            await context.Response.WriteAsJsonAsync(errorDetails);

        }
    }
    else
    {
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync("html/index.html");
    }

});


app.Run();
