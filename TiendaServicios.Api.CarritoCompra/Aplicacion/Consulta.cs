﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {

        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }

        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto > 
        {
            private readonly CarritoContexto _contexto;
            private readonly ILibroService _libroService;


            public Manejador(CarritoContexto contexto, ILibroService libroService)
            {
                _contexto = contexto;
                _libroService = libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                List<CarritoDetalleDto> listaDetalleDto = new List<CarritoDetalleDto>();

                var carritoSesion = await _contexto.CarritoSesion
                    .FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);

                var carritoSesionDetalle = await _contexto.CarritoSesionDetalle
                    .Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                foreach (var libro in carritoSesionDetalle)
                {
                   var response =    await _libroService.GetLibro(new Guid(libro.ProductoSeleccionado));

                   if (response.resultado)
                   {
                       var objetoLibro = response.libro;
                       var carritoDetalle = new CarritoDetalleDto
                       {
                           TituloLibro = objetoLibro.Titulo,
                           FechaPublicacion = objetoLibro.FechaPublicacion,
                           LibroId = objetoLibro.LibreriaMaterialId
                       };

                       listaDetalleDto.Add(carritoDetalle);
                   }
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaDetalleDto
                };

                return carritoSesionDto;
            }
        }
    }
}
