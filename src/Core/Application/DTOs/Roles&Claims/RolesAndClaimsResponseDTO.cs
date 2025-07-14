using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RolesAndClaimsResponseDTO
    {
        public List<string> Roles { get; set; }
        public List<ClaimDTO> Claims { get; set; } 
    }
}
