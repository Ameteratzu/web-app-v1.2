﻿namespace DGPCE.Sigemad.Domain.Modelos;

public class Territorio
{
    public Territorio() { }
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;
    public virtual ICollection<Incendio> Incendios { get; set; } = new List<Incendio>();
}
