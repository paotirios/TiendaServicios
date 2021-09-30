using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Test
{
    public class LibrosServiceTest
    {

        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);

            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }
        

        [Fact]
        public void GetLibros()
        {
            // que metodo de mi microservices se estta engarando de sacar la consulta de libros de la base de datos


            // 1. emular a la instaania de entity framework core
            // para emular se en un ambiente de unit test se usan objetos de tipo mock,
            // se disfraza de actor
            var mockContexto = new Mock<ContextoLibreria>();

            // 2. emular al mapping
            var mockMapping = new Mock<IMapper>();

            // 3. instanciar a la clase manejador y pasarle los mocks creados
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mockMapping.Object);



        }

    }
}
