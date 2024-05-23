﻿namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app)
        {
            app.MapGet("estudantes", 
                handler:() => new Estudante(nome: "Alan"));
        }
    }
}
