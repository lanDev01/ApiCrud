using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app)
        {
            // var rotasEstudantes:RouteGroupBuilder RouteGroupBuilder = app.MapGroup(prefix: "estudantes");
            // Cria um grupo de rotas com o prefixo "estudantes"
            var rotasEstudantes = app.MapGroup("estudantes");

            // Criar Novo Estudante
            rotasEstudantes.MapPost("", async (AddEstudanteRequest request, AppDbContext context) =>
            {
                var jaExiste = await context.Estudantes.AnyAsync(estudante => estudante.Nome == request.Nome);

                if (jaExiste)
                    return Results.Conflict(error: "Já existe!");

                var novoEstudante = new Estudante(request.Nome);
                await context.Estudantes.AddAsync(novoEstudante);
                await context.SaveChangesAsync();
                
                return Results.Ok(novoEstudante);
            });

            // Listar Estudantes
            rotasEstudantes.MapGet("", async (AppDbContext context) =>
            {
                var estudantes = await context
                    .Estudantes
                    .Where(estudante => estudante.Ativo)
                    .ToListAsync();

                return estudantes;
            });
        }
    }
}
