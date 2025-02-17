using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t4tea.data.Entities;

namespace t4tea.service.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(ApplicationUser user);

    }
}
