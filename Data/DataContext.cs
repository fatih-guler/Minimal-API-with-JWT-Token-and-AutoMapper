using MinimalAPI.Model;

namespace MinimalAPI.Data
{
    /// <summary>
    /// In this project, no database used. 
    /// Data is provided statically
    /// </summary>
    public class DataContext
    {
        public List<User> Users = new List<User>()
        {
            new User()
            {
                Id = 1,
                Name = "Alex",
                LastName = "Souza",
                Password = "123456",
                Username = "alexsouza",
                Email = "alexsouza@mail.com"
            },
            new User()
            {
                Id = 2,
                Name = "Jim",
                LastName = "Turney",
                Password = "ABC6789",
                Username = "jimturney",
                Email = "jimturney@mail.com"
            },
            new User()
            {
                Id = 3,
                Name = "Alan",
                LastName = "Turing",
                Password = "TestPss",
                Username = "alanturing",
                Email = "alanturing@mail.com"
            },
            new User()
            {
                Id = 4,
                Name = "Steve",
                LastName = "Wozniak",
                   Password = "I am Wozniak",
                Email = "heysteve@mail.com",
                Username = "heysteve"
            },
            new User()
            {
                Id = 5,
                Name = "Martin",
                LastName = "Fowler",
                Password = "psss",
                Username = "martinfowler",
                Email = "martinfowler@mail.com"
            }
        };
        public List<SecurityRoleName> Roles = new List<SecurityRoleName>()
        {
            new SecurityRoleName()
            {
                Name =  "Accountant",
                UserId = 1
            },
            new SecurityRoleName()
            {
                Name =  "Admin",
                UserId = 2
            },
            new SecurityRoleName()
            {
                Name =  "Test",
                UserId = 3
            },
            new SecurityRoleName()
            {
                Name =  "Test",
                UserId = 4
            },
            new SecurityRoleName()
            {
                Name =  "Admin",
                UserId = 5
            }
        };
        public List<SecurityUserAction> Actions = new List<SecurityUserAction>()
        {
            new SecurityUserAction()
            {
                UserId = 1,
                ActionNumberTotal = 63
            },
            new SecurityUserAction()
            {
                UserId = 2,
                ActionNumberTotal = 2
            },
            new SecurityUserAction()
            {
                UserId = 3,
                ActionNumberTotal = 2
            },
            new SecurityUserAction()
            {
                UserId = 4,
                ActionNumberTotal = 2
            },
            new SecurityUserAction()
            {
                UserId = 5,
                ActionNumberTotal = 14
            } 
        };
    }
}
