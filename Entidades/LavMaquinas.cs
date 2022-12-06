using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class LavMaquinas
    {
        private int idMaquina { get; set; }
        public int IdMaquina
        {
            get { return idMaquina; }
            set { idMaquina = value; }
        }

        public string CodigoQR { get; set; }
        public byte[] CodigoQRImagen { get; set; }

        private string modelo;
        public string Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }

        private string numeroSerie;
        public string NumeroSerie
        {
            get { return numeroSerie; }
            set { numeroSerie = value; }
        }
        private int idMarca;

        public int IdMarca
        {
            get { return idMarca; }
            set { idMarca = value; }
        }
        private string marca;

        public string Marca
        {
            get { return marca; }
            set { marca = value; }
        }

        public string Estado { get; set; }

        public bool EstadoOK { get; set; }

        public string FechaEstimadaResolucion{ get; set; }
    }
}
