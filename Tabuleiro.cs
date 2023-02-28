namespace DotsGame;

public class Tabuleiro
{
    private const int TamTabuleiro = 5; 
    private const char EspacoVazio = ' ';
    private const char CaracterDePonto = '*';
    private const char Jogador1 = '1'; 
    private const char Jogador2 = '2'; 

    public char[,] TabuleiroCompleto { get; private set; }
    public Tabuleiro(){
        TabuleiroCompleto = new char[5, 5];
        ResetaTabuleiro();
    }

    private void ResetaTabuleiro()
    {
        LimparTabuleiro();
        PreencherPontos();
        LimparJogadas();
        LimparPontuacao();
    }

    private void PreencherPontos()
    {
        for(int i = 0; i < TamTabuleiro; i++)
            if(i % 2 == 0)
                for(int j = 0; j < TamTabuleiro; j++)
                    if(j % 2 == 0)
                        TabuleiroCompleto[i, j] = '*';
    }

    private void LimparJogadas()
    {
        for(int i = 0; i < TamTabuleiro; i++)
        {
            if(i % 2 == 0){
                for(int j = 0; j < TamTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = ' ';
            } else {
                for(int k = 0; k < TamTabuleiro; k++)
                    if(k % 2 == 0)
                        TabuleiroCompleto[i, k] = ' ';
            }
        }
    }

    private void LimparPontuacao()
    {
        for(int i = 0; i < TamTabuleiro; i++)
            if(i % 2 == 1)
                for(int j = 0; j < TamTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = ' ';
    }

    public void ExibirTabuleiro()
    {
        Console.WriteLine("Exibindo tabuleiro...\n");

        for (int i = 0; i < TamTabuleiro; i++)
        {
            for (int j = 0; j < TamTabuleiro; j++)
            {
                Console.Write(" " + TabuleiroCompleto[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void ExibirTabuleiroComJogadasPossiveis()
    {
        Console.WriteLine("Exibindo tabuleiro com jogadas possíveis...\n");
        
        Console.WriteLine("*   1  *   2   *\n");
        Console.WriteLine("3   *  4   *   5\n");
        Console.WriteLine("*   6  *   7   *\n");
        Console.WriteLine("8   *  9   *   10\n");
        Console.WriteLine("*  11  *   12  *\n");
    }

    public void LimparTabuleiro()
    {
        for (int i = 0; i < TamTabuleiro; i++)
            for (int j = 0; j < TamTabuleiro; j++)
                TabuleiroCompleto[i, j] = ' ';
    }

    public void ExibeTabuleiroDoTutorial()
    {
        PreencherPontos();
        PreencheExemploDeJogadas();
        PreencheExemploDePontuacao();
        ExibirTabuleiro();
    }

    private void PreencheExemploDeJogadas()
    {
        for(int i = 0; i < TamTabuleiro; i++)
        {
            if(i % 2 == 0){
                for(int j = 0; j < TamTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = '-';
            } else {
                for(int k = 0; k < TamTabuleiro; k++)
                    if(k % 2 == 0)
                        TabuleiroCompleto[i, k] = '|';
            }
        }
    }

    private void PreencheExemploDePontuacao()
    {
        for(int i = 0; i < TamTabuleiro; i++)
            if(i % 2 == 1)
                for(int j = 0; j < TamTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = '2';
    }

    public void MarcarJogada(bool jogador)
    {
        if(JogadasFinalizadas())
            return;

        Console.WriteLine("Vez do Jogador: " + (jogador ? "Humano" : "Máquina"));
        Console.WriteLine("Onde você quer jogar(número):");
        string numero = Console.ReadLine() ?? "";

        // Traduz o numero da tela para posição (x, y) da matriz
        var coordenada = MapearPosicao(int.Parse(numero));

        while(!CoordenadasSaoValidas(coordenada))
        { 
            if(TabuleiroCompleto[coordenada.linha, coordenada.coluna] == (jogador ? 'x' : '='))
                Console.WriteLine("Outro jogador já fez essa jogada!");

            Console.WriteLine("Digite um valor válido:");
            numero = Console.ReadLine() ?? "";
            coordenada = MapearPosicao(int.Parse(numero));
        }

        TabuleiroCompleto[coordenada.linha, coordenada.coluna] = jogador ? '=' : 'x';

        VerificarPontuacao(coordenada, jogador);
    }

    private (int linha, int coluna) MapearPosicao(int posicao)
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

    public bool JogadasFinalizadas()
        => (TabuleiroCompleto[1, 1] != ' ' 
            && TabuleiroCompleto[1, 3] != ' ' 
            && TabuleiroCompleto[3, 1] != ' ' 
            && TabuleiroCompleto[3, 3] != ' ');

    private bool CoordenadasSaoValidas((int linha, int coluna) coordenada)
        => (TabuleiroCompleto[coordenada.linha, coordenada.coluna] == ' ');

    private void VerificarPontuacao((int linha, int coluna) coordenada, bool jogador)
    {
        // Verifica pra esquerda, se puder
        if(coordenada.coluna - 2 >= 0 && (coordenada.linha - 1 >= 0 && coordenada.linha + 1 < TamTabuleiro))
            VerificaQuadradoAEsquerda(coordenada.linha, coordenada.coluna, jogador);

        // Verifica pra direita, se puder
        if(coordenada.coluna + 2 < TamTabuleiro && (coordenada.linha - 1 >= 0 && coordenada.linha + 1 < TamTabuleiro))
            VerificaQuadradoADireita(coordenada.linha, coordenada.coluna, jogador);

        // //Verifica pra cima, se puder
        if(coordenada.linha - 2 >= 0 && (coordenada.coluna - 1 >= 0 && coordenada.coluna + 1 < TamTabuleiro))
            VerificaQuadradoAcima(coordenada.linha, coordenada.coluna, jogador);

        //Verifica pra baixo, se puder
        if(coordenada.linha + 2 < TamTabuleiro && (coordenada.coluna - 1 >= 0 && coordenada.coluna + 1 < TamTabuleiro))
            VerificaQuadradoAbaixo(coordenada.linha, coordenada.coluna, jogador);
    }

    private void VerificaQuadradoAEsquerda(int linha, int coluna, bool jogador)
    {
        if(TabuleiroCompleto[linha, coluna] != EspacoVazio
            && TabuleiroCompleto[linha, coluna - 2] != EspacoVazio
            && TabuleiroCompleto[linha + 1, coluna - 1] != EspacoVazio
            && TabuleiroCompleto[linha - 1, coluna - 1] != EspacoVazio
            && TabuleiroCompleto[linha, coluna - 1] != CaracterDePonto)
        {
            TabuleiroCompleto[linha, coluna - 1] = SimboloDoJogador(jogador);
            LiberarOutraJogada(jogador);
        }
    }

    private void VerificaQuadradoADireita(int linha, int coluna, bool jogador)
    {
        if(TabuleiroCompleto[linha, coluna] != EspacoVazio
            && TabuleiroCompleto[linha, coluna + 2] != EspacoVazio
            && TabuleiroCompleto[linha + 1, coluna + 1] != EspacoVazio
            && TabuleiroCompleto[linha - 1, coluna + 1] != EspacoVazio
            && TabuleiroCompleto[linha, coluna + 1] != CaracterDePonto)
        {
            TabuleiroCompleto[linha, coluna + 1] = SimboloDoJogador(jogador);
            LiberarOutraJogada(jogador);
        }
    }

    private void VerificaQuadradoAcima(int linha, int coluna, bool jogador)
    {
        if(TabuleiroCompleto[linha, coluna] != EspacoVazio
            && TabuleiroCompleto[linha - 2, coluna] != EspacoVazio
            && TabuleiroCompleto[linha - 1, coluna + 1] != EspacoVazio
            && TabuleiroCompleto[linha - 1, coluna - 1] != EspacoVazio
            && TabuleiroCompleto[linha - 1, coluna] != CaracterDePonto)
        {
            TabuleiroCompleto[linha - 1, coluna] = SimboloDoJogador(jogador);
            LiberarOutraJogada(jogador);
        }
    }
    private void VerificaQuadradoAbaixo(int linha, int coluna, bool jogador)
    {
        if(TabuleiroCompleto[linha, coluna] != EspacoVazio
            && TabuleiroCompleto[linha + 2, coluna] != EspacoVazio
            && TabuleiroCompleto[linha + 1, coluna + 1] != EspacoVazio
            && TabuleiroCompleto[linha + 1, coluna - 1] != EspacoVazio
            && TabuleiroCompleto[linha + 1, coluna] != CaracterDePonto)
        {
            TabuleiroCompleto[linha + 1, coluna] = SimboloDoJogador(jogador);
            LiberarOutraJogada(jogador);
        }
    }

    private void LiberarOutraJogada(bool jogador)
    {
        System.Console.WriteLine("Parabéns, você joga de novo!");
        MarcarJogada(jogador);
    }

    private char SimboloDoJogador(bool jogador)
        => jogador ? Jogador1 : Jogador2;
}