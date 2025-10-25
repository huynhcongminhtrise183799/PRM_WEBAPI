using PRM.Application.Model;
using PRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM.Application.IService
{
    public interface IUserDeviceTokenService
    {
        Task<bool> AddToken(UserDeviceTokenModel userDeviceToken);
    }
}
