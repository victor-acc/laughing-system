using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class Permiso
{
    public int Idpermiso { get; set; }

    public string? NombrePermisos { get; set; }

    public string? EstadoPermisos { get; set; }

    public string? Descripcion { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();
}
