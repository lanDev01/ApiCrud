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
            rotasEstudantes.MapPost("", 
                async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
                {
                    var jaExiste = await context.Estudantes.AnyAsync(estudante => estudante.Nome == request.Nome, ct);

                    if (jaExiste)
                        return Results.Conflict(error: "Já existe!");

                    var novoEstudante = new Estudante(request.Nome);
                    await context.Estudantes.AddAsync(novoEstudante, ct);
                    await context.SaveChangesAsync(ct);

                    var estudantesRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);
                    return Results.Ok(estudantesRetorno);
                });

            // Listar Estudantes
            rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) =>
            {
                var estudantes = await context
                    .Estudantes
                    .Where(estudante => estudante.Ativo)
                    .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
                    .ToListAsync(ct);

                return estudantes;
            });

            // Atualizar Nome do Estudante
            rotasEstudantes.MapPut("{id:guid}",
                async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
                {
                    var estudante = await context.Estudantes.FirstOrDefaultAsync(estudante => estudante.Id == id);

                    if (estudante == null)
                        return Results.NotFound();

                    estudante.AtualizarNome(request.Nome);

                    await context.SaveChangesAsync(ct);

                    return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
                });

            // Deletar
            rotasEstudantes.MapDelete("{id}",
                async (Guid id, AppDbContext context, CancellationToken ct) => 
                {
                    var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id);

                    if (estudante == null)
                        return Results.NotFound();

                    estudante.Desativar();

                    await context.SaveChangesAsync(ct);
                    return Results.Ok();
                });
        }
    }
}
