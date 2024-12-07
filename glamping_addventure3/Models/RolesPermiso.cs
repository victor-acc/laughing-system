using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class RolesPermiso
{
    public int IdrolPermiso { get; set; }

    public int? Idrol { get; set; }

    public int? Idpermiso { get; set; }

    public virtual Permiso? IdpermisoNavigation { get; set; }

    public virtual Role? IdrolNavigation { get; set; }
}
