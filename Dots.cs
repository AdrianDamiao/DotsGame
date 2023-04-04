using DotsGame.Extensions;

namespace DotsGame;

public class Dots
{
    private const int NumeroJogadasPossiveis = 12;
    private const int TamanhoDoTabuleiro = 5;
    
    private Tabuleiro tabuleiro = new();

    private char[,] TabuleiroOficial = new char[5, 5];
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

        // Avalia o min e max
        AvaliaMiniMax(noRaiz, jogador);


        // realiza as jogadas
        var resultado = Jogar(noRaiz, jogador);
        if(resultado == 1 || resultado == -1 || resultado == 0)
            return;

        // // Não finalizado ainda
        // while(!noRaiz.JogadasFinalizadas())
        // {
        //     RealizarJogada(noRaiz, jogador);
        // }

        // ExibirJogadorVencedor();
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

    private int EncontrarGanhador(No no)
    {
        if(!no.JogadasFinalizadas()) {
            return 2; // Jogo não finalizado ainda.
        }
        
        var locaisDePontuacao = new List<(int linha, int coluna)>(){
            (1, 1), (1, 3), (3, 1), (3, 3),
        };

        int pontuacaoJogador = 0;
        int pontuacaoMaquina = 0;

        foreach(var local in locaisDePontuacao)
        {
            if(no.Tabuleiro[local.linha, local.coluna] == '1')
                pontuacaoJogador++;
            else if(no.Tabuleiro[local.linha, local.coluna] == '2')
                pontuacaoMaquina++;
        }

        if(pontuacaoJogador == pontuacaoMaquina)
            return 0;

        return pontuacaoJogador > pontuacaoMaquina ? -1 : 1; 
    }

    private void PreencheArvoreDePossibilidades(No no, bool[] jogadasPossiveis, bool jogador)
    {
        for(int i = 0; i < jogadasPossiveis.Length; i++)
        {
            // Jogada é possivel?
            if(jogadasPossiveis[i] == true)
            {   
                No filho = new No();

                // Copia o tabuleiro do Pai
                filho.CopiaMatriz(no);

                // Realiza Jogada do filho
                var coordenada = filho.MapearPosicao(i+1);
                filho.Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';

                jogadasPossiveis[i] = false;
                no.Filhos.Add(filho);

                if(jogador) 
                    filho.ValorMinMax = Int32.MinValue;
                else
                    filho.ValorMinMax = Int32.MaxValue; 

                if(filho.VerificarPontuacao(coordenada, jogador) == true)
                {
                    PreencheArvoreDePossibilidades(filho, jogadasPossiveis, jogador);
                } else {
                    if(jogador) {
                        PreencheArvoreDePossibilidades(filho, jogadasPossiveis, !jogador);
                    }
                    else 
                    {
                        PreencheArvoreDePossibilidades(filho, jogadasPossiveis, !jogador);
                    }
                }
                jogadasPossiveis[i] = true;
            }
        }
    }

    private int AvaliaMiniMax(No no, bool jogador)
    {
        var ganhador = EncontrarGanhador(no);

        if (ganhador != 2) // Se o jogo foi finalizado
        {
            if (ganhador == 1)
            {
                return 1;
            }
            else if (ganhador == -1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (jogador)
            {
                for (int i = 0; i < no.Filhos.Count; i++)
                {
                    var resultado = AvaliaMiniMax(no.Filhos[i], !jogador);
                    if (resultado < no.ValorMinMax)
                        no.ValorMinMax = resultado;
                }
                return no.ValorMinMax;
            }
            else if (!jogador)
            {
                for (int i = 0; i < no.Filhos.Count; i++)
                {
                    var resultado = AvaliaMiniMax(no.Filhos[i], !jogador);
                    if (resultado > no.ValorMinMax)
                        no.ValorMinMax = resultado;
                }
                return no.ValorMinMax;
            }
        }
        return 0;
    }

    private int Jogar(No no, bool jogador)
    {
        //Atualiza o tabuleiro e imprime
        ExibirTabuleiro();

        var ganhador = EncontrarGanhador(no);
        if (ganhador != 2)
        {
            System.Console.WriteLine("Ganhador: "+ ganhador);                
        } 
        else
        {
            if (!jogador) //Maquina
            {    
                int melhorPontuacao = Int32.MinValue;
                int indiceMelhorFilho = 0;
                for (int i = 0; i < no.Filhos.Count; i++)
                {
                    if (no.Filhos[i].ValorMinMax > melhorPontuacao)
                    {
                        melhorPontuacao = no.Filhos[i].ValorMinMax;
                        indiceMelhorFilho = i;
                    }
                }

                var coordenadaDaJogada = (0, 0);

                for(int i = 0; i < 5; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        if(no.Filhos[indiceMelhorFilho].Tabuleiro[i, j] != TabuleiroOficial[i, j]
                            && no.Filhos[indiceMelhorFilho].Tabuleiro[i, j] != '2'
                            && no.Filhos[indiceMelhorFilho].Tabuleiro[i, j] != '1')
                        {
                            coordenadaDaJogada = (i, j);
                        }
                    }
                }

                AtualizarTabuleiro(no.Filhos[indiceMelhorFilho].Tabuleiro);

                //Exibe jogada
                Thread.Sleep(2000);


                if(no.Filhos[indiceMelhorFilho].VerificarPontuacao(coordenadaDaJogada, false) == true)
                {
                    System.Console.WriteLine("IA marcou ponto");
                    Jogar(no.Filhos[indiceMelhorFilho], jogador);
                }
                else
                {
                    Jogar(no.Filhos[indiceMelhorFilho], !jogador);
                }
            }
            else
            {
                int indiceFilhoCorreto = 0;

                Console.WriteLine("Onde você quer jogar(número):");
                string numero = Console.ReadLine() ?? "";

                // Traduz o numero da tela para posição (x, y) da matriz
                var coordenada = MapearPosicao(int.Parse(numero));

                while(no.Tabuleiro[coordenada.linha, coordenada.coluna] != ' ')
                { 
                    if(no.Tabuleiro[coordenada.linha, coordenada.coluna] == 'x')
                        Console.WriteLine("Outro jogador já fez essa jogada!");

                    Console.WriteLine("Digite um valor válido:");
                    numero = Console.ReadLine() ?? "";
                    coordenada = MapearPosicao(int.Parse(numero));
                }

                TabuleiroOficial[coordenada.linha, coordenada.coluna] = 'x';

                foreach (var (filho, index) in no.Filhos.LoopIndex())
                {
                    if (ComparaTabuleiro(filho.Tabuleiro))
                    {
                        indiceFilhoCorreto = index;
                    }
                }
                // no.Filhos[indiceFilhoCorreto].Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';

                if(no.Filhos[indiceFilhoCorreto].VerificarPontuacao(coordenada, true) == true)
                {
                    System.Console.WriteLine("Player marcou ponto");
                    Jogar(no.Filhos[indiceFilhoCorreto], jogador);
                }
                else
                {
                    Jogar(no.Filhos[indiceFilhoCorreto], !jogador);
                }
            }
        }
        return 0;
    }

    private void AtualizarTabuleiro(char[,] tabuleiroNovo)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                TabuleiroOficial[i, j] = tabuleiroNovo[i,j];
            }
        }
    }

    public bool ComparaTabuleiro(char[,] jogoDoNo)
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                if(jogoDoNo[i, j] != TabuleiroOficial[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ExibirTabuleiro()
    {
        Console.WriteLine("Exibindo tabuleiro...\n");

        for (int i = 0; i < TamanhoDoTabuleiro; i++)
        {
            for (int j = 0; j < TamanhoDoTabuleiro; j++)
            {
                Console.Write(" " + TabuleiroOficial[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public (int linha, int coluna) MapearPosicao(int posicao)
        => posicao switch {
            1 => (0, 1),
            2 => (0, 3),
            3 => (1, 0),
            4 => (1, 2),
            5 => (1, 4),
            6 => (2, 1),
            7 => (2, 3),
            8 => (3, 0),
            9 => (3, 2),
            10 => (3, 4),
            11 => (4, 1),
            12 => (4, 3),
            _ => default,
        };
}