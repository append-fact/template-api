using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    
    public class RolesWithPermissionsResponseDTO
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
