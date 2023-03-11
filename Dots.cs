namespace DotsGame;

public class Dots
{
    private const int NumeroJogadasPossiveis = 12;
    private const int TamanhoDoTabuleiro = 5;
    
    private Tabuleiro tabuleiro = new();
    private No noRaiz = new No();
    private bool[] JogadasPossiveis { get; set; } = new bool[12]{
        true, true, true, true,
        true, true, true, true,
        true, true, true, true,
    };

    public void Inicializar()
    {
        // Console.Clear();
        Console.WriteLine("Jogo Iniciado...");

        // Thread.Sleep(TimeSpan.FromSeconds(3));

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

        No no = new();
        no.ExibeTabuleiroDoTutorial();
    }

    public void IniciarJogo()
    {
        //Verificar a possibilidade de exibir as jogadas possíveis conforme o andamento do jogo
        ExibirTabuleiroComJogadasPossiveis();

        // Força primeira jogada da IA na posição 1
        var coordenada = noRaiz.MapearPosicao(1);
        bool jogador = false;
        JogadasPossiveis[0] = false;
        noRaiz.Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';

        // Espera o jogador fazer a segunda jogada
        Console.WriteLine("Onde você quer jogar(número):");
        string numero = Console.ReadLine() ?? "";
        coordenada = noRaiz.MapearPosicao(int.Parse(numero));
        noRaiz.Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';
        JogadasPossiveis[int.Parse(numero) - 1] = false;
        
        // Após jogar duas vezes, começa a preencher a árvore
        if(JogadasPossiveis.Count(jogada => jogada == false) >= 2)
        {
            PreencheArvoreDePossibilidades(noRaiz, JogadasPossiveis, jogador);
        }

        // Não finalizado ainda
        while(!noRaiz.JogadasFinalizadas())
        {
            RealizarJogada(noRaiz, jogador);
        }

        ExibirJogadorVencedor();
    }


    private void RealizarJogada(No noRaiz, bool jogador)
    {
        if(noRaiz.JogadasFinalizadas())
            return;

        noRaiz.MarcarJogada(jogador);

        noRaiz.ExibirTabuleiro();
        RealizarJogada(noRaiz, !jogador);
    }

    private void ExibirTabuleiroComJogadasPossiveis()
    {
        No no = new();
        no.ExibirTabuleiroComJogadasPossiveis();
    }

    private void ExibirJogadorVencedor()
    {
        var pontuacoes = new List<(int linha, int coluna)>(){
            (1, 1), (1, 3), (3, 1), (3, 3),
        };

        int pontuacaoJogador1 = 0;
        int pontuacaoJogador2 = 0;

        foreach(var ponto in pontuacoes)
        {
            if(tabuleiro.TabuleiroCompleto[ponto.linha, ponto.coluna] == 1)
                pontuacaoJogador1++;
            else
                pontuacaoJogador2++;
        }

        System.Console.WriteLine($"Jogador {(pontuacaoJogador1 > pontuacaoJogador2 ? "1" : "2")} Venceu!\n");
    }

    private void PreencheArvoreDePossibilidades(No no, bool[] jogadasPossiveis, bool jogador)
    {
        for(int i = 0; i < jogadasPossiveis.Length; i++)
        {
            // Jogada é possivel?
            if(jogadasPossiveis[i] == true)
            {
                No filho = new No();

                // Realiza Jogada
                // var coordenada = filho.MapearPosicao(i+1);
                // filho.Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';
                
                jogadasPossiveis[i] = false;
                no.Filhos.Add(filho);

                // filho.ValorMinMax = int.MinValue;
                PreencheArvoreDePossibilidades(filho, jogadasPossiveis, jogador);
                jogadasPossiveis[i] = true;
            }
        }
    }
}