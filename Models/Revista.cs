public class Revista : MaterialBiblioteca
{
    public int MesPublicacao { get; set; }

    public int Edicao { get; set; }

    public override int CalcularPrazo()
    {
        return 7;
    }

    public override string Descricao()
    {
        return $"Revista: {Titulo}, Edição: {Edicao}, Mês: {MesPublicacao}";
    }
}







// public class Revista : MaterialBiblioteca
// {

//     private int _mesPublicacao;
//     public int MesPublicacao
//     {
//         get { return _mesPublicacao; }
//         set
//         {
//             if (value < 1 || value > 12)
//             {
//                 Console.WriteLine("O mês de publicação deve estar entre 1 e 12.");
//                 return;
//             }
//             _mesPublicacao = value;
//         }
//     }

//     public int _edicao { get; set; }

//     public int Edicao 
//     {
//         get { return _edicao; }
//         set 
//         {
//             if (value <= 0)
//             {
//                 Console.WriteLine("O número da edição deve ser maior que 0.");
//                 return;
//             }
//             _edicao = value;
//         }
//     }
//     public override int CalcularPrazo()
//     {
//         return 7;
//     }
//     public override string Descricao()
//     {
//         return $"Revista: {Titulo}, Edição: {Edicao}, Mês: {MesPublicacao}";
//     }
// }