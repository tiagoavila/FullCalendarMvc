using System;
using System.Linq;
using System.Web.Mvc;
using FullCalendarMvc.DAL;
using FullCalendarMvc.Models;

namespace FullCalendarMvc.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarEventos(string start, string end)
        {
            //código para trazer os eventos do mês
            DateTime dataInicial = Convert.ToDateTime(start);
            DateTime dataFinal = Convert.ToDateTime(end);
            var eventosDb = _unitOfWork.EventoRepositorio.ListarEventos(dataInicial, dataFinal);

            var eventos = from e in eventosDb
                          select new
                                     {
                                         id = e.Id,
                                         title = e.Titulo,
                                         start = e.DataInicial.ToString("yyyy-MM-ddTHH:mm:ssZ").Replace("T00:00:00Z", string.Empty),
                                         end = e.DataFinal != null ? e.DataFinal.Value.ToString("yyyy-MM-ddTHH:mm:ssZ") : string.Empty,
                                         color = e.Cor
                                     };

            return Json(eventos.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AdicionarEvento(string title, string start)
        {
            DateTime dataInicial = Convert.ToDateTime(start);
            var evento = new Evento
            {
                Titulo = title,
                DataInicial = dataInicial,
                DataFinal = null,
                Cor = "#337ab7"
            };
            _unitOfWork.EventoRepositorio.Adicionar(evento);
        }

        [HttpPost]
        public void AtualizarEvento(string id, string newEventStart, string newEventEnd, string visualizacaoAtual)
        {
            DateTime? dataFinal = null;

            //Se estiver na visualição de Mês não pode considerar o horário, somente a data pois nesta visualização o evento é 
            //tratado como evento de dia inteiro, senão fizer isto a atualização mostrará o 
            //evento como se ele tivesse no horário de 00:00 do dia
            if (visualizacaoAtual.Equals("month"))
            {
                //Se a data contém T00:00:00.000Z tem que remover antes de converter para DateTime, 
                //pois se não após converter volta um dia
                if (newEventStart.Contains("T00:00:00.000Z"))
                    newEventStart = newEventStart.Replace("T00:00:00.000Z", string.Empty);

                if (!string.IsNullOrEmpty(newEventEnd))
                {
                    if (newEventEnd.Contains("T00:00:00.000Z"))
                    {
                        newEventEnd = newEventEnd.Replace("T00:00:00.000Z", string.Empty);
                        dataFinal = Convert.ToDateTime(newEventEnd);
                    }
                }
            }
            
            //Tem que remover o .000Z, senão converte a hora de forma errada
            newEventStart = newEventStart.Replace(".000Z", string.Empty);
            if (!string.IsNullOrEmpty(newEventEnd))
            {
                newEventEnd = newEventEnd.Replace(".000Z", string.Empty);
                dataFinal = Convert.ToDateTime(newEventEnd);
            }

            DateTime dataInicial = Convert.ToDateTime(newEventStart);

            Evento evento = _unitOfWork.EventoRepositorio.ConsultarPorId(Convert.ToInt32(id));
            evento.DataInicial = dataInicial;
            evento.DataFinal = dataFinal;
            _unitOfWork.EventoRepositorio.Atualizar(evento);
        }

        [HttpPost]
        public ActionResult EditarEvento(string id, string titulo, string cor)
        {
            Evento evento = _unitOfWork.EventoRepositorio.ConsultarPorId(Convert.ToInt32(id));
            evento.Titulo = titulo;
            evento.Cor = "#" + cor;
            _unitOfWork.EventoRepositorio.Atualizar(evento);
            return Json(new {Sucesso = true}, JsonRequestBehavior.AllowGet);
        }
    }
}
