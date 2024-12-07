using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class Role
{
    public int Idrol { get; set; }

    public string? Nombre { get; set; }

    public string? Estado { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
