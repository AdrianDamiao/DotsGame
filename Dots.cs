namespace DotsGame;

public class Dots
{
    private Tabuleiro tabuleiro = new();
    public void Inicializar()
    {
        // Console.Clear();
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
        // Console.Clear();

        //Verificar a possibilidade de exibir as jogadas possíveis conforme o andamento do jogo
        ExibirTabuleiroComJogadasPossiveis();

        Console.WriteLine("Quem começará jogando? 1 - Humano | 2 - IA");
        int jogador = int.Parse(Console.ReadLine() ?? "");
        
        if (jogador == 1 || jogador == 2)
        {
            while(!tabuleiro.JogadasFinalizadas())
            {
                RealizarJogada(tabuleiro, jogador == 1 ? true : false);
            }
        }

        ExibirJogadorVencedor();
    }


    private void RealizarJogada(Tabuleiro tabuleiro, bool jogador)
    {
        if(tabuleiro.JogadasFinalizadas())
            return;
        tabuleiro.MarcarJogada(jogador);

        tabuleiro.ExibirTabuleiro();
        RealizarJogada(tabuleiro, !jogador);
    }

    private void ExibirTabuleiroComJogadasPossiveis()
    {
        Tabuleiro tabuleiro = new Tabuleiro();
        tabuleiro.ExibirTabuleiroComJogadasPossiveis();
    }

    private void ExibirJogadorVencedor()
    {
        System.Console.WriteLine("Jogador 1 Venceu!\n");
    }
}