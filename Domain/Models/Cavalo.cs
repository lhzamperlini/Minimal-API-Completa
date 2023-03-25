using ApiAluguelCavalos.Domain.Models.Base;
using Microsoft.Win32;

namespace ApiAluguelCavalos.Domain.Models;

public class CavaloModel : EntidadeBase
{	
	public string Nome { get; private set; }
    public int Registro { get; private set; }
    public string Raca { get; private set;}
	public string Sexo { get; private set;}
	public string Pelagem { get; private set; }

    public CavaloModel(string nome, int registro, string raca, string sexo, string pelagem) 
	{
		Nome = nome;
		Registro = registro;
		Raca = raca;
		Sexo = sexo;
		Pelagem = pelagem;
	}

	public void EditarInformacoes(string nome, int registro, string raca, string sexo, string pelagem)
	{
		Nome = nome;
        Registro = registro;
        Raca = raca;
        Sexo = sexo;
		Pelagem = pelagem;
    }

}
