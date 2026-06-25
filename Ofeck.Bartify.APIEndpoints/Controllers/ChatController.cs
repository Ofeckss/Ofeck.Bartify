using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Chats;
using Ofeck.Bartify.Core.Chats.Requests;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatController: ControllerBase
{
    private readonly ChatService chatService;
    
    public ChatController(ChatService chatService)
    {
        this.chatService = chatService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateChatRequest request)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var nuevoChat = await this.chatService.Create(request, usuarioId);

            return this.Created(nuevoChat, new { url = nuevoChat, mensaje = "Chat creado exitosamente." });
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetById()
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
            
            var list = await this.chatService.GetByUser(usuarioId);
            
            return this.Ok(list);
            
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }
}