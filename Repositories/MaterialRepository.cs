public class MaterialRepository : IMaterialRepository 
{ 
    private Dictionary<int, MaterialBiblioteca> _materiais = new Dictionary<int, MaterialBiblioteca>() 
    { 
        { 
            1, new Livro { Id = 1, Titulo = "O Pequeno Príncipe", AnoPublicacao = 2020 } 
        },
        {
            2, new Revista{ Id = 2, Titulo = "VOGUE", MesPublicacao =  5}
        }
    };

    public IEnumerable<MaterialBiblioteca> ObterTodos()
    {
        return _materiais.Values;
    }

    public MaterialBiblioteca? ObterPorId(int id)
    {
        if (!_materiais.ContainsKey(id))
        {
            return null;
        }
        return _materiais[id];
    }

        public MaterialBiblioteca Adicionar(MaterialBiblioteca material)
        {
            int novoId = GerarNovoId();
            
            material.Id = novoId;

            _materiais.Add(material.Id, material);

            return material;

        }

    private int GerarNovoId()
    {
        if (_materiais.Count == 0)
            return 1;

        return _materiais.Keys.Max() + 1;
    }

    public bool Atualizar(int id, MaterialBiblioteca materialAtualizado)
    {
        if(!_materiais.ContainsKey(id))
        {
            return false;
        }

        materialAtualizado.Id = id;

        _materiais[id] = materialAtualizado;

        return true;
    }

    public bool Remover(int id)
    {
        return _materiais.Remove(id);
    }
    
}
