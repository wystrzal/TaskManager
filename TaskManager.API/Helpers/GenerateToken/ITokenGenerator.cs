using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Helpers.GenerateToken
{
     public interface ITokenGenerator
    {
        string GenerateJwtToken(User user, IConfiguration config);
    }
}
