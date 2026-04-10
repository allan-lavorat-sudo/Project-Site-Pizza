using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PizzaDelivery.API.Services;
using PizzaDelivery.API.DTOs;
using System.Security.Claims;

namespace PizzaDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService service, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Register new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(UserRegisterDto dto)
    {
        try
        {
            var result = await _service.RegisterAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Usuário registrado com sucesso"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao registrar usuário",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// User login
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(UserLoginDto dto)
    {
        try
        {
            var result = await _service.LoginAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Login realizado com sucesso"
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao fazer login",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Logout
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);
            await _service.LogoutAsync(userId);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Logout realizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer logout");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao fazer logout",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService service, ILogger<OrdersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Create new order
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> Create(CreateOrderDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);
            var order = await _service.CreateAsync(dto, userId);

            return CreatedAtAction(nameof(GetById), new { id = order.Id },
                new ApiResponse<OrderDto>
                {
                    Success = true,
                    Data = order,
                    Message = "Pedido criado com sucesso"
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
            _logger.LogError(ex, "Erro ao criar pedido");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao criar pedido",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(int id)
    {
        try
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Pedido não encontrado"
                });

            return Ok(new ApiResponse<OrderDto>
            {
                Success = true,
                Data = order,
                Message = "Pedido obtido com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter pedido");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao obter pedido",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get user's orders
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetUserOrders()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);
            var orders = await _service.GetUserOrdersAsync(userId);

            return Ok(new ApiResponse<List<OrderDto>>
            {
                Success = true,
                Data = orders,
                Message = "Pedidos do usuário listados com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pedidos");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar pedidos",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Update order status (Admin/Manager only)
    /// </summary>
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request)
    {
        try
        {
            if (!Enum.TryParse<PizzaDelivery.API.Models.OrderStatus>(request.Status, out var status))
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Status inválido"
                });

            var order = await _service.UpdateStatusAsync(id, status);
            return Ok(new ApiResponse<OrderDto>
            {
                Success = true,
                Data = order,
                Message = "Status do pedido atualizado com sucesso"
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
            _logger.LogError(ex, "Erro ao atualizar status do pedido");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao atualizar status do pedido",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get pending orders (Admin/Manager only)
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetPendingOrders()
    {
        try
        {
            var orders = await _service.GetPendingOrdersAsync();
            return Ok(new ApiResponse<List<OrderDto>>
            {
                Success = true,
                Data = orders,
                Message = "Pedidos pendentes listados com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pedidos pendentes");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar pedidos pendentes",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}

[ApiController]
[Route("api/v1/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly IPromotionService _service;
    private readonly ILogger<PromotionsController> _logger;

    public PromotionsController(IPromotionService service, ILogger<PromotionsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get active promotions
    /// </summary>
    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<PromotionDto>>>> GetActive()
    {
        try
        {
            var promotions = await _service.GetActiveAsync();
            return Ok(new ApiResponse<List<PromotionDto>>
            {
                Success = true,
                Data = promotions,
                Message = "Promoções ativas listadas com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar promoções ativas");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar promoções",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get all promotions
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<PromotionDto>>>> GetAll()
    {
        try
        {
            var promotions = await _service.GetAllAsync();
            return Ok(new ApiResponse<List<PromotionDto>>
            {
                Success = true,
                Data = promotions,
                Message = "Promoções listadas com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar promoções");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao listar promoções",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Get promotion by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> GetById(int id)
    {
        try
        {
            var promotion = await _service.GetByIdAsync(id);
            if (promotion == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Promoção não encontrada"
                });

            return Ok(new ApiResponse<PromotionDto>
            {
                Success = true,
                Data = promotion,
                Message = "Promoção obtida com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter promoção");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao obter promoção",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Create promotion (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> Create(CreatePromotionDto dto)
    {
        try
        {
            var promotion = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = promotion.Id },
                new ApiResponse<PromotionDto>
                {
                    Success = true,
                    Data = promotion,
                    Message = "Promoção criada com sucesso"
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar promoção");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao criar promoção",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Update promotion (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> Update(int id, CreatePromotionDto dto)
    {
        try
        {
            var promotion = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<PromotionDto>
            {
                Success = true,
                Data = promotion,
                Message = "Promoção atualizada com sucesso"
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
            _logger.LogError(ex, "Erro ao atualizar promoção");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao atualizar promoção",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Delete promotion (Admin only)
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
                    Message = "Promoção não encontrada"
                });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Promoção deletada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar promoção");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao deletar promoção",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}

// Helper DTOs
public class UpdateOrderStatusRequest
{
    public string Status { get; set; } = string.Empty;
}
