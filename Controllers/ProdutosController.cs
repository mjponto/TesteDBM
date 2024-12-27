using Microsoft.AspNetCore.Mvc;
using TesteDBM.DTOs;
using TesteDBM.Models;
using TesteDBM.Services;

namespace TesteDBM.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var produtos = await _produtoService.ObterTodosAsync();
            return Ok(produtos);
        }

        // GET: api/produtos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        // POST: api/produtos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoCreateDto dto)
        {
            var produto = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                DataCadastro = DateTime.Now 
            };

            try
            {
                var novoProduto = await _produtoService.AdicionarAsync(produto);
                return CreatedAtAction(nameof(Get), new { id = novoProduto.Id }, novoProduto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("O ID da rota não corresponde ao ID do DTO.");
            
            var produto = new Produto
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco
            };

            try
            {
                var produtoAtualizado = await _produtoService.AtualizarAsync(id, produto);
                if (produtoAtualizado == null)
                    return NotFound();
                
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _produtoService.RemoverAsync(id);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }