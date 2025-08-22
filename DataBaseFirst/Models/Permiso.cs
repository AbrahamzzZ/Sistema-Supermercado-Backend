using System;
using System.Collections.Generic;

namespace DataBaseFirst.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public int? IdRol { get; set; }

    public int? IdMenu { get; set; }

    public virtual Menu? IdMenuNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
