using LibraryAPI.DAL;
using LibraryAPI.DTOs;
using LibraryAPI.DTOs.Book;
using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext context;

        public BooksController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await context.Books.Include(b=>b.Genre).ToListAsync();
            List<BookGetDTO> result = new List<BookGetDTO>();
            foreach (var item in data)
            {
                BookGetDTO dto = new BookGetDTO(item.Id,item.Title,item.SalePrice,item.CostPrice,item.Genre.Name);
                result.Add(dto);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1) return BadRequest();
            var data = await context.Books.Include(g=>g.Genre).FirstOrDefaultAsync(g => g.Id == id);
            if (data == null) return NotFound();
            BookGetDTO dto = new BookGetDTO(data.Id, data.Title, data.SalePrice, data.CostPrice,data.Genre.Name);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO dto)
        {
            if (dto == null) return BadRequest();
            if (string.IsNullOrEmpty(dto.Title) || dto.SalePrice<0 || dto.CostPrice<0) return BadRequest();
            Book book = new Book()
            {
                Title = dto.Title  ,
                SalePrice = dto.SalePrice ,
                CostPrice = dto.CostPrice ,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                GenreId = dto.GenreId ,
                IsDeleted = false
            };
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return Created(new Uri($"/api/Books/{book.Id}", UriKind.Relative), book);

        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO dto)
        {
            if (dto == null) return BadRequest();
            if (string.IsNullOrEmpty(dto.Title) || dto.SalePrice < 0 || dto.CostPrice < 0) return BadRequest();
            var data = await context.Books.FirstOrDefaultAsync(g => g.Id == id);
            if (data == null) return NotFound();
            data.ModifyDate = DateTime.Now;
            data.Title = dto.Title;
            data.SalePrice = dto.SalePrice;
            data.CostPrice = dto.CostPrice;
            data.GenreId = dto.GenreId;
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1) return BadRequest();
            var data = await context.Books.FirstOrDefaultAsync(g => g.Id == id);
            if (data == null) return NotFound();
            context.Books.Remove(data);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
