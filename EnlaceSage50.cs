using sage._50;
using sage.ew.articulo;
using sage.ew.stocks;
using sage.ew.usuario;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnlaceSage50Servicio
{
    public class EnlaceSage50 : IEnlaceSage50
    {
        public EventLog Log { get; set; }

        public bool EjecutableSage50(string tipoOperacion, string empresa, string articulo, string talla, string color, string almaOrigen, string almaDestino, int unidades, string observaciones)
        {
            bool resultado = false;
            Log = new EventLog();
            if (!EventLog.SourceExists("EnlaceSage50Service"))
                EventLog.CreateEventSource("EnlaceSage50Service", "");
            Log.Source = "EnlaceSage50Service";
            string resLlamada = "(" + tipoOperacion + "," + empresa + "," + articulo + "," + talla + "," + color + "," + almaOrigen + "," + almaDestino + "," + unidades + ")";
            Log.WriteEntry("EjecutableSage50 " + resLlamada, EventLogEntryType.Information);
            if (tipoOperacion == "TR")
            {
                resultado = RealizarTraspasoMercancia(empresa, articulo, talla, color, almaOrigen, almaDestino, unidades, observaciones);
                if (resultado)
                {
                    Log.WriteEntry("Resultado RealizarTraspasoMercancia TRUE", EventLogEntryType.Information);
                }
                else
                {
                    Log.WriteEntry("Resultado RealizarTraspasoMercancia FALSE", EventLogEntryType.Warning);
                }
            }
            return resultado;
        }

        public bool RealizarTraspasoMercancia(string empresa,string articulo, string talla, string color, string almaOrigen, string almaDestino, int unidades, string observaciones)
        {
            bool resultado = false;
            try
            {
                //Log = new EventLog();            
                //if (!EventLog.SourceExists("EnlaceSage50Service"))                
                //    EventLog.CreateEventSource("EnlaceSage50Service", "");
                //Log.Source = "EnlaceSage50Service";                           

                //Creación documento albarán de traspaso de Sage 50
                Log.WriteEntry("RealizarTraspasoMercancia(P1) -> ", EventLogEntryType.Information);
                StockAlbTraspaso docAlbTraspaso = new StockAlbTraspaso();
                Log.WriteEntry("RealizarTraspasoMercancia(P2) -> ", EventLogEntryType.Information);
                docAlbTraspaso._New();
                docAlbTraspaso._Numero = docAlbTraspaso._Obten_Nuevo_Numero();
                docAlbTraspaso._Fecha = DateTime.Today;
                docAlbTraspaso._AlmacenOrigen = almaOrigen;
                docAlbTraspaso._AlmacenDestino = almaDestino;
                docAlbTraspaso._Observaciones = observaciones;
                Log.WriteEntry("RealizarTraspasoMercancia(P3) -> ", EventLogEntryType.Information);
                //docAlbTraspaso._Usuario._Codigo = "SUPERVISOR";
                //Creación de la línea de detalle del documento de albarán de traspaso
                StockAlbTraspaso.ArticuloStockAlbTraspaso lineaDocTr = new StockAlbTraspaso.ArticuloStockAlbTraspaso(docAlbTraspaso);
                sage.ew.articulo.Articulo loArt = new Articulo(articulo, talla, color);
                Log.WriteEntry("RealizarTraspasoMercancia(P4) -> ", EventLogEntryType.Information);
                bool tieneTyC = false;
                if ((talla != "") || (color != "")) { tieneTyC = true; }
                lineaDocTr._Add(loArt._Codigo, unidades, talla, color);
                lineaDocTr._Valoracion = 0;

                //Cálculo del coste para valorar la línea
                decimal coste = loArt._Coste_Ultimo_Talla_Color(
                    new sage.ew.articulo.DataAccess.Clases.DatosCalculoCosteUltimoDto()
                    {
                        _Articulo = loArt._Codigo,
                        _Almacen = almaOrigen,
                        _Talla = talla,
                        _Color = color,
                        _Empresa = empresa,
                        _Con_TallasyColores = tieneTyC,
                        _Fecha = DateTime.Today
                    });
                lineaDocTr._Coste = coste;
                //Guardar la línea en el documento
                docAlbTraspaso._AddLinea(lineaDocTr);
                
                //Guardar el documento
                bool estad = docAlbTraspaso._Save();
                resultado = true;
                //}
            }
            catch (Exception ex)
            {
                Log.WriteEntry("RealizarTraspasoMercancia() -> " + ex.Message, EventLogEntryType.Error);
                resultado = false;
            }

            return resultado;
            


        }

        public bool hola()
        {
            //sage.ew.db.DB.Conexion
            Log.WriteEntry("Hola. Prueba conexión ", EventLogEntryType.Error);
            return true;
        }
    }
}
