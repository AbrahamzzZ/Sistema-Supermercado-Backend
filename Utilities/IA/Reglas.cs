using System.Text.RegularExpressions;

namespace Utilities.IA
{
    public class Reglas
    {
        public static TipoAnalisis DetectarTipoAnalisis(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return TipoAnalisis.Invalido;

            prompt = prompt.ToLower();

            var regexVendido = new Regex(
                @"\b(vendid[oa]s?|ventas?|facturaci[oó]n|ingresos?)\b",
                RegexOptions.IgnoreCase
            );

            var regexComprado = new Regex(
                @"\b(comprad[oa]s?|compras?|abastecimiento|adquisiciones?)\b",
                RegexOptions.IgnoreCase
            );

            var regexProducto = new Regex(
                @"\b(producto|productos|art[ií]culo|art[ií]culos|item|items|mercanc[ií]a)\b",
                RegexOptions.IgnoreCase
            );

            var regexCategoria = new Regex(
                @"\b(categor[ií]a|categor[ií]as|rubro|rubros|secci[oó]n|secciones|departamento)\b",
                RegexOptions.IgnoreCase
            );

            var regexCliente = new Regex(
                @"\b(cliente|clientes|comprador|compradores|consumidor|consumidores|frecuentes)\b",
                RegexOptions.IgnoreCase
            );

            var regexProveedor = new Regex(
                @"\b(proveedor|proveedores|distribuidor|distribuidores)\b",
                RegexOptions.IgnoreCase
            );

            var regexSucursal = new Regex(
                @"\b(sucursal|sucursales|tienda|tiendas|local|locales)\b",
                RegexOptions.IgnoreCase
            );

            bool esProducto = regexProducto.IsMatch(prompt);
            bool esCategoria = regexCategoria.IsMatch(prompt);
            bool esCliente = regexCliente.IsMatch(prompt);
            bool esProveedor = regexProveedor.IsMatch(prompt);
            bool esSucursal = regexSucursal.IsMatch(prompt);
            bool esVendido = regexVendido.IsMatch(prompt);
            bool esComprado = regexComprado.IsMatch(prompt);

            if (esProducto)
                return TipoAnalisis.Producto;

            if (esCategoria)
                return TipoAnalisis.Categoria;

            if (esCliente)
                return TipoAnalisis.Cliente;

            if (esProveedor)
                return TipoAnalisis.Proveedor;

            if (esSucursal)
                return TipoAnalisis.Sucursal;

            if (esVendido)
                return TipoAnalisis.Vendido;

            if (esComprado)
                return TipoAnalisis.Comprado;

            return TipoAnalisis.Invalido;
        }

        public enum TipoAnalisis
        {
            Vendido,
            Comprado,
            Producto,
            Categoria,
            Cliente,
            Proveedor,
            Sucursal,
            Invalido
        }
    }
}
