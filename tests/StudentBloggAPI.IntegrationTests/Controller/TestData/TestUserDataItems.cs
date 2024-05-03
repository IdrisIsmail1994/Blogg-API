using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBloggAPI.IntegrationTests.Controller.TestData;
public class TestUserDataItems
{
    public static IEnumerable<object[]> GetTestUsers => new List<object[]>
    {
        new object[]
        { 
            new TestUser 
            { 
                Base64EncodedUsernamePassword = "b2xhOk8xYW5vcm1hbm4j",
                User = new Models.Entities.User
                {
                    Created = new DateTime(2023, 11, 14, 9, 30, 0),
                    Email = "ola@gmail.com",
                    FirstName = "Ola",
                    UserName = "ola",
                    LastName = "Normann",
                    Id = 1,
                    IsAdminUser = false,
                    Salt = "$2a$13$HkriD/2KXmhHmfBJIM5cFO",
                    HashedPassword = "$2a$13$HkriD/2KXmhHmfBJIM5cFOVCQvSigHBCeWk1G2Qv068u6oSf/v52G",
                    Updated = new DateTime(2023, 11, 14, 9, 30, 0)
                }
            }
        },
        new object[]
        { 
            new TestUser
            { 
                Base64EncodedUsernamePassword = "a2FyaTpLMXJpbm9ybWFubiM=",
                User = new Models.Entities.User
                {
                    Created = new DateTime(2023, 11, 14, 9, 30, 0),
                    Email = "kari@gmail.com",
                    FirstName = "Kari",
                    UserName = "kari",
                    LastName = "Normann",
                    Id = 2,
                    IsAdminUser = false,
                    Salt = "$2a$13$qOtZSfQ4FWjn.zBUEhukV.",
                    HashedPassword = "$2a$13$qOtZSfQ4FWjn.zBUEhukV.4wKuxw4VwrpQMs4inUg/I7DMwdcyEQm",
                    Updated = new DateTime(2023, 11, 14, 9, 30, 0)
                }
            }
        }
    };
}
