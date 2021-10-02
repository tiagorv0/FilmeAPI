using FilmesAPI.Controllers.Models;
using FilmesAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//instalar para o banco de dados: Microsoft.EntityFrameworkCore / Microsoft.EntityFrameworkCore.Tools / MySql.Data.EntityFrameworkCore

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        //private static List<Filme> filmes = new List<Filme>(); // nao precisa mais devido ao banco de dados
        //private static int id = 1; // nao precisa mais devido ao banco de dados
        private FilmeContext _context;

        public FilmeController(FilmeContext context)
        {
            _context = context;
        }

        [HttpPost] //pega os dados 
        public IActionResult AdicionaFilme([FromBody] Filme filme)
        {
            //filme.Id = id++;
            //filmes.Add(filme);
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmesPorID), new { Id = filme.Id }, filme); //retorna o local
        }

        [HttpGet] //envia os dados
        public IEnumerable<Filme> RecuperaFilmes()
        {
            return _context.Filmes;
        }

        [HttpGet("{id}")] //envia um parametro para requisição
        public IActionResult RecuperaFilmesPorID(int id)
        {
            
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme != null)
            {
                return Ok(filme);
            }
            return NotFound();
        }

        [HttpPut("{id}")] //atualiza 
        public IActionResult AtualizaFilme(int id, [FromBody] Filme filmeNovo)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme == null)
            {
                return NotFound();

            }
            filme.Titulo = filmeNovo.Titulo;
            filme.Genero = filmeNovo.Genero;
            filme.Diretor = filmeNovo.Diretor;
            filme.Duracao = filmeNovo.Duracao;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")] //deleta
        public IActionResult DeletaFilme(int id)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null)
            {
                return NotFound();

            }

            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
