using System.Collections.Generic;
using System.Data;
using System.Linq;
using FullCalendarMvc.Models;
using System;
using System.Data.Objects;

namespace FullCalendarMvc.DAL
{
    public class EventoRepositorio
    {
        readonly FullCalendarMvcContext _context;

        public EventoRepositorio(FullCalendarMvcContext contexto)
        {
            _context = contexto;
        }

        public List<Evento> ListarEventos(DateTime dataInicial, DateTime dataFinal)
        {
            return _context.Eventos.Where(x => x.DataInicial >= dataInicial 
                && (x.DataFinal == null || x.DataFinal <= dataFinal)).ToList();
        }

        public void Adicionar(Evento evento)
        {
            _context.Eventos.Add(evento);
            _context.Salvar();
        }

        public Evento ConsultarPorId(int id)
        {
            return _context.Eventos.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void Atualizar(Evento evento)
        {
            _context.Entry(evento).State = EntityState.Modified;
            _context.Salvar();
        }
    }
}