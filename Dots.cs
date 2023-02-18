namespace DotsGame;

public class Dots
{
    public void Inicializar()
    {
        Console.Clear();
        Console.WriteLine("Jogo Iniciado...");

        Thread.Sleep(TimeSpan.FromSeconds(3));

        string opcaoDigitada;
        int opcaoNumero;
        do{
            Console.WriteLine("Seleciona uma opção:");
            Console.WriteLine("1 - Tutorial \n2 - Iniciar Jogo \n3 - Sair");

            opcaoDigitada = Console.ReadLine() ?? "";
            if (int.TryParse(opcaoDigitada, out opcaoNumero)) {
                switch(opcaoNumero){
                    case 1: ExibirTutorial(); break;
                    case 2: IniciarJogo(); break;
                    case 3: Console.WriteLine("Saindo..."); break;
                    default: Console.WriteLine("Opção inválida!"); break;
                }
            } else {
                Console.WriteLine("Opção inválida!");
            }
        }while(opcaoNumero != 3);
    }

    public void ExibirTutorial()
    {
        Console.WriteLine("Exibindo tutorial...");
        Console.WriteLine("O jogo Dots é composto por 9 pontos....");

        Tabuleiro tabuleiro = new();
        tabuleiro.ExibeTabuleiroDoTutorial();
    }

    public void IniciarJogo()
    {
        Console.Clear();
        
        Tabuleiro tabuleiro = new Tabuleiro();
        tabuleiro.ExibirTabuleiroComJogadasPossiveis();
        
        Console.WriteLine("Quem começará jogando? 1 - Humano | 2 - IA");
        int jogador = int.Parse(Console.ReadLine() ?? "");

        if(jogador == 1 || jogador == 2) {
            RealizarJogada(tabuleiro, jogador);
        } else {
            Console.WriteLine("Opção inválida!");
        }
    }

    private void RealizarJogada(Tabuleiro tabuleiro, int jogador)
    {
        Console.WriteLine("Onde você quer jogar(número):");
        string numero = Console.ReadLine() ?? "";

        tabuleiro.MarcarJogada(int.Parse(numero));

        tabuleiro.ExibirTabuleiro();
    }
}