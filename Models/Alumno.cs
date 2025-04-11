using System.ComponentModel.DataAnnotations;

namespace MvcAppAws_Daniel_delaCruz.Models
{
    public class Alumno
    {
        [Key]
        public int ID_Alumno { get; set; }
        public string NombreCompleto { get; set; }
        public string Matricula { get; set; } //Ej.- 076-MAT
        public string Carrera { get; set; } //Nombre de la Carrera : Matemáticas
        public string EmpresaAsignada { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Estado Estado { get; set; }
    }

    public enum Estado
    {
        Activo = 1,
        Inactivo = 2,
        Esperando = 3,
    }

}
