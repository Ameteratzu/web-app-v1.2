﻿namespace DGPCE.Sigemad.Domain.Modelos;
public class Organismo
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public Administracion Administracion { get; set; }
    public int IdAdministracion { get; set; }
}
