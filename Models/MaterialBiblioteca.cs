public abstract class MaterialBiblioteca
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public int AnoPublicacao { get; set; }

    public abstract int CalcularPrazo();

    public virtual string Descricao()
    {
        return $"Título: {Titulo}, Ano de Publicação: {AnoPublicacao}";
    }
}





// public abstract class MaterialBiblioteca
// {
//     public int Id { get; set; }

//     public string Titulo { get; set;} = string.Empty;

//     private int _anoPublicacao;
//     public int AnoPublicacao 
//     {
//         get { return _anoPublicacao; }
//         set 
//         {
//             if (value < 0)
//             {
//                 Console.WriteLine("O ano de publicação não pode ser negativo.");
//                 return;
//             }
//             _anoPublicacao = value;
//         }
//     }


//     public abstract int CalcularPrazo();

//     public virtual string Descricao()
//     {
//         return $"Título: {Titulo}, Ano de Publicação: {AnoPublicacao}";
//     }

// }