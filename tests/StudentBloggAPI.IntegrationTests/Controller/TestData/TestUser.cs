using StudentBloggAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentBloggAPI.IntegrationTests.Controller.TestData;
public class TestUser
{
    public User? User { get; set; }
    public string Base64EncodedUsernamePassword { get; init; } = string.Empty;
}
