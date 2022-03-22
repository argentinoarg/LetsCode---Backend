using BACK.Data;
using BACK.Filters;
using BACK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACK.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly KanbanDbContext _context;

        public CardsController(KanbanDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardModel>>> Listar()
        {
            return await _context.Cards.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CardModel>> Incluir(string lista, string titulo, string conteudo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var cardModel = new CardModel()
            {
                List = lista,
                Title = titulo,
                Content = conteudo
            };

            await _context.Cards.AddAsync(cardModel);
            await _context.SaveChangesAsync(true);

            return Ok(cardModel);
        }

        [TrackExecutionTime]
        [HttpPut("{id}")]
        public async Task<ActionResult<CardModel>> Alterar(int id, CardModel cardModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
            {
                return NotFound();
            }
            else
            {
                card.Title = cardModel.Title;
                card.Content = cardModel.Content;
                card.List = cardModel.List;
            }

            await _context.SaveChangesAsync(true);

            return Ok(card);
        }

        [TrackExecutionTime]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CardModel>> Remover(int id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
            {
                return NotFound();
            }
            else
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync(true);
            }

            return Ok(_context.Cards);
        }
    }
}
