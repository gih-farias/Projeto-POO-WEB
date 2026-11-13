public class Livro : MaterialBiblioteca, IEmprestimo
{
    public string Autor { get; set; } = string.Empty;

    public int Paginas { get; set; }

    public override int CalcularPrazo()
    {
        return 14;
    }

    public override string Descricao()
    {
        return $"Livro: {Titulo}, Autor: {Autor}";
    }

    public string Emprestar()
    {
        return $"O livro {Titulo}, escrito pelo autor(a): {Autor} pode ser emprestado por {CalcularPrazo()} dias.";
    }
}





// public class Livro : MaterialBiblioteca, IEmprestimo
// {
//     public string Autor { get; set; } = string.Empty;

//     private int _paginas;
//     public int Paginas 
//     {
//         get { return _paginas; }
//         set 
//         {
//             if (value <= 0)
//             {
//                 Console.WriteLine("O número de páginas deve ser maior que 0.");
//                 return;
//             }
//             _paginas = value;
//         }
//     }
    
//     public override int CalcularPrazo()
//     {
//         return 14;
//     }

//     public override string Descricao()
//     {
//         return $"Livro: {Titulo}";
//     }

//     public string Emprestar()
//     {
//         return $"O livro {Titulo}, escrito pelo autor(a):{Autor} pode ser emprestado por {CalcularPrazo()} dias.";
//     }
// }