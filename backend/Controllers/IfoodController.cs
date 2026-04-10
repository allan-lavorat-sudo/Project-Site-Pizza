using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PizzaDelivery.API.Services;
using PizzaDelivery.API.DTOs;

namespace PizzaDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class IfoodController : ControllerBase
{
    private readonly IIfoodService _service;
    private readonly ILogger<IfoodController> _logger;

    public IfoodController(IIfoodService service, ILogger<IfoodController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get Ifood authorization URL
    /// </summary>
    [HttpGet("auth-url")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<string>>> GetAuthUrl()
    {
        try
        {
            var url = await _service.GetAuthorizationUrlAsync();
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Data = url,
                Message = "URL de autorização obtida com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter URL de autorização");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao obter URL de autorização",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Authenticate with Ifood
    /// </summary>
    [HttpPost("authenticate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> Authenticate([FromBody] AuthenticateIfoodRequest request)
    {
        try
        {
            var success = await _service.AuthenticateAsync(request.AuthCode);
            return Ok(new ApiResponse<bool>
            {
                Success = success,
                Data = success,
                Message = success ? "Autenticado com Ifood com sucesso" : "Falha na autenticação"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao autenticar com Ifood");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao autenticar com Ifood",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Webhook receiver for Ifood events
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> ProcessWebhook(
        [FromBody] IfoodOrderEventDto eventData,
        [FromHeader(Name = "X-IFOOD-Signature")] string signature)
    {
        try
        {
            // TODO: Validate signature
            var success = await _service.ProcessWebhookAsync(eventData);
            return Ok(new ApiResponse<bool>
            {
                Success = success,
                Data = success,
                Message = "Webhook processado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar webhook do Ifood");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao processar webhook",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Sync menu with Ifood
    /// </summary>
    [HttpPost("sync-menu")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> SyncMenu()
    {
        try
        {
            var success = await _service.SyncMenuAsync();
            return Ok(new ApiResponse<bool>
            {
                Success = success,
                Data = success,
                Message = success ? "Menu sincronizado com sucesso" : "Falha na sincronização"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao sincronizar menu");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao sincronizar menu",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Update order status in Ifood
    /// </summary>
    [HttpPut("orders/{ifoodOrderId}/status")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatus(
        string ifoodOrderId,
        [FromBody] UpdateIfoodOrderStatusRequest request)
    {
        try
        {
            var success = await _service.UpdateOrderStatusAsync(ifoodOrderId, request.Status);
            return Ok(new ApiResponse<bool>
            {
                Success = success,
                Data = success,
                Message = "Status atualizado no Ifood com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar status no Ifood");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Erro ao atualizar status",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}

// Helper DTOs
public class AuthenticateIfoodRequest
{
    public string AuthCode { get; set; } = string.Empty;
}

public class UpdateIfoodOrderStatusRequest
{
    public string Status { get; set; } = string.Empty;
}
