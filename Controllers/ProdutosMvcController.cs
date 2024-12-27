using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using TesteDBM.DTOs;
using TesteDBM.Models;

namespace TesteDBM.Controllers
{
    public class ProdutosMvcController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProdutosMvcController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            
            _httpClient.BaseAddress = new Uri("http://localhost:5295");
        }

        // GET: /ProdutosMvc/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/produtos");
            if (!response.IsSuccessStatusCode)
            {
                return View("ErrorApi");
            }

            var produtos = await response.Content.ReadFromJsonAsync<List<Produto>>();
            return View(produtos);
        }

        // GET: /ProdutosMvc/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ProdutosMvc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Envia POST /api/produtos com o DTO
            var response = await _httpClient.PostAsJsonAsync("api/produtos", model);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Erro ao criar produto via API: {errorMessage}");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProdutosMvc/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Chama GET /api/produtos/{id} na API
            var response = await _httpClient.GetAsync($"api/produtos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var produto = await response.Content.ReadFromJsonAsync<Produto>();
            if (produto == null)
                return NotFound();
            
            var dto = new ProdutoUpdateDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco
            };

            return View(dto);
        }

        // POST: /ProdutosMvc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoUpdateDto model)
        {
            if (id != model.Id)
            {
                ModelState.AddModelError("", "O ID da rota não corresponde ao ID do objeto.");
                return View(model);
            }

            // Chama PUT /api/produtos/{id} para atualizar no banco
            var response = await _httpClient.PutAsJsonAsync($"api/produtos/{id}", model);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Erro ao atualizar produto via API: {errorMessage}");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ProdutosMvc/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"api/produtos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var produto = await response.Content.ReadFromJsonAsync<Produto>();
            if (produto == null)
                return NotFound();

            return View(produto);
        }

        // POST: /ProdutosMvc/DeleteConfirmed/5
        [HttpPost]
        [ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/produtos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Erro ao excluir produto via API: {errorMessage}");
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
