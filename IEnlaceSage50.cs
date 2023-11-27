using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EnlaceSage50Servicio
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IEnlaceSage50
    {
        [OperationContract]
        bool EjecutableSage50(string tipoOperacion, string empresa, string articulo, string talla, string color, string almaOrigen, string almaDestino, int unidades, string observaciones); // Define los métodos que quieras exponer en tu servicio
        [OperationContract]
        bool RealizarTraspasoMercancia(string empresa, string articulo, string talla, string color, string almaOrigen, string almaDestino, int unidades, string observaciones);
        [OperationContract]
        bool hola();
    }
}
