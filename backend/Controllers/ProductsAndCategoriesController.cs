using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PizzaDelivery.API.Services;
using PizzaDelivery.API.DTOs;
using System.Security.Claims;

namespace PizzaDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService service, ILogger<ProductsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all active products
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAll()
    {
        try
        {
            var products = await _service.GetAllAsync();
            return Ok(new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Data = products,
                Message = "Produtos listados com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar produtos",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    [HttpGet("category/{categoryId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetByCategory(int categoryId)
    {
        try
        {
            var products = await _service.GetByCategoryAsync(categoryId);
            return Ok(new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Data = products,
                Message = $"Produtos da categoria {categoryId} listados com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos por categoria");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar produtos",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
    {
        try
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Produto não encontrado"
                });

            return Ok(new ApiResponse<ProductDto>
            {
                Success = true,
                Data = product,
                Message = "Produto obtido com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter produto");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao obter produto",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Create new product (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Create(CreateProductDto dto)
    {
        try
        {
            var product = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id },
                new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = product,
                    Message = "Produto criado com sucesso"
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao criar produto",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Update product (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, UpdateProductDto dto)
    {
        try
        {
            var product = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<ProductDto>
            {
                Success = true,
                Data = product,
                Message = "Produto atualizado com sucesso"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao atualizar produto",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Delete product (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Produto não encontrado"
                });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Produto deletado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar produto");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao deletar produto",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService service, ILogger<CategoriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAll()
    {
        try
        {
            var categories = await _service.GetAllAsync();
            return Ok(new ApiResponse<List<CategoryDto>>
            {
                Success = true,
                Data = categories,
                Message = "Categorias listadas com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar categorias");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar categorias",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get category by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(int id)
    {
        try
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Categoria não encontrada"
                });

            return Ok(new ApiResponse<CategoryDto>
            {
                Success = true,
                Data = category,
                Message = "Categoria obtida com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter categoria");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao obter categoria",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Create new category (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Create(CreateCategoryDto dto)
    {
        try
        {
            var category = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id },
                new ApiResponse<CategoryDto>
                {
                    Success = true,
                    Data = category,
                    Message = "Categoria criada com sucesso"
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar categoria");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao criar categoria",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Update category (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Update(int id, CreateCategoryDto dto)
    {
        try
        {
            var category = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<CategoryDto>
            {
                Success = true,
                Data = category,
                Message = "Categoria atualizada com sucesso"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar categoria");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao atualizar categoria",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Delete category (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Categoria não encontrada"
                });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Categoria deletada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar categoria");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao deletar categoria",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}
