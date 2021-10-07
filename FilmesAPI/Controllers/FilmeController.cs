using FilmesAPI.Models;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;


//instalar para o banco de dados: Microsoft.EntityFrameworkCore / Microsoft.EntityFrameworkCore.Tools / MySql.Data.EntityFrameworkCore

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        //private static List<Filme> filmes = new List<Filme>(); // nao precisa mais devido ao banco de dados
        //private static int id = 1; // nao precisa mais devido ao banco de dados
        private AppDbContext _context;
        private IMapper _mapper;

        public FilmeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost] //pega os dados 
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            /*Filme filme = new Filme
            {
                Titulo = filmeDto.Titulo,
                Genero = filmeDto.Genero,
                Diretor = filmeDto.Diretor,
                Duracao = filmeDto.Duracao
            };*/ Filme filme = _mapper.Map<Filme>(filmeDto);
            //filme.Id = id++;
            //filmes.Add(filme);
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmesPorID), new { Id = filme.Id }, filme); //retorna o local
        }

        [HttpGet] //envia os dados
        public IActionResult RecuperaFilmes([FromQuery] int? classificacaoEtaria = null)
        {
            List<Filme> filmes;
            if (classificacaoEtaria == null){
                filmes = _context.Filmes.ToList();
            }
            else { 
            
            filmes = _context.Filmes.Where(filme => filme.ClassificacaoEtaria <= classificacaoEtaria).ToList();
            }

            if(filmes != null)
            {
                List<ReadFilmeDto> readDto = _mapper.Map<List<ReadFilmeDto>>(filmes);
                return Ok(readDto);
            }

            return NotFound();
        }

        [HttpGet("{id}")] //envia um parametro para requisição
        public IActionResult RecuperaFilmesPorID(int id)
        {
            
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme != null)
            {
                /*ReadFilmeDto filmeDto = new ReadFilmeDto
                {
                    Titulo = filme.Titulo,
                    Diretor = filme.Diretor,
                    Duracao = filme.Duracao,
                    Id = filme.Id,
                    Genero = filme.Genero,
                    HoraDaConsulta = DateTime.Now
                };*/ ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
                return Ok(filmeDto);
            }
            return NotFound();
        }

        [HttpPut("{id}")] //atualiza 
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if(filme == null)
            {
                return NotFound();

            }
            /*filme.Titulo = filmeDto.Titulo;
            filme.Genero = filmeDto.Genero;
            filme.Diretor = filmeDto.Diretor;
            filme.Duracao = filmeDto.Duracao;*/ _mapper.Map(filmeDto, filme);
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
