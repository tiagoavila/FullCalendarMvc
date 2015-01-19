using System;
using System.Data.Entity.Validation;

namespace FullCalendarMvc.DAL
{
    public class UnitOfWork : IDisposable
    {
        private bool _disposed;
        private readonly FullCalendarMvcContext _context = new FullCalendarMvcContext();
        private EventoRepositorio _eventoRepositorio;


        public EventoRepositorio EventoRepositorio
        {
            get { return _eventoRepositorio ?? (_eventoRepositorio = new EventoRepositorio(_context)); }
        }

        //public void Salvar()
        //{
        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }

        //}

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}