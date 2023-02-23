namespace DotsGame;

public class Tabuleiro
{
    private const int tamanhoDoTabuleiro = 5; 
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
        for(int i = 0; i < tamanhoDoTabuleiro; i++)
            if(i % 2 == 0)
                for(int j = 0; j < tamanhoDoTabuleiro; j++)
                    if(j % 2 == 0)
                        TabuleiroCompleto[i, j] = '*';
    }

    private void LimparJogadas()
    {
        for(int i = 0; i < tamanhoDoTabuleiro; i++)
        {
            if(i % 2 == 0){
                for(int j = 0; j < tamanhoDoTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = ' ';
            } else {
                for(int k = 0; k < tamanhoDoTabuleiro; k++)
                    if(k % 2 == 0)
                        TabuleiroCompleto[i, k] = ' ';
            }
        }
    }

    private void LimparPontuacao()
    {
        for(int i = 0; i < tamanhoDoTabuleiro; i++)
            if(i % 2 == 1)
                for(int j = 0; j < tamanhoDoTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = ' ';
    }

    public void ExibirTabuleiro()
    {
        Console.WriteLine("Exibindo tabuleiro...\n");

        for (int i = 0; i < tamanhoDoTabuleiro; i++)
        {
            for (int j = 0; j < tamanhoDoTabuleiro; j++)
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
        for (int i = 0; i < tamanhoDoTabuleiro; i++)
            for (int j = 0; j < tamanhoDoTabuleiro; j++)
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
        for(int i = 0; i < tamanhoDoTabuleiro; i++)
        {
            if(i % 2 == 0){
                for(int j = 0; j < tamanhoDoTabuleiro; j++)
                    if(j % 2 == 1)
                        TabuleiroCompleto[i, j] = '-';
            } else {
                for(int k = 0; k < tamanhoDoTabuleiro; k++)
                    if(k % 2 == 0)
                        TabuleiroCompleto[i, k] = '|';
            }
        }
    }

    private void PreencheExemploDePontuacao()
    {
        for(int i = 0; i < tamanhoDoTabuleiro; i++)
            if(i % 2 == 1)
                for(int j = 0; j < tamanhoDoTabuleiro; j++)
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
        var coordenadas = MapearPosicao(int.Parse(numero));

        while(!CoordenadasSaoValidas(coordenadas))
        { 
            if(TabuleiroCompleto[coordenadas.x, coordenadas.y] == (jogador ? 'x' : '='))
                Console.WriteLine("Outro jogador já fez essa jogada!");

            Console.WriteLine("Digite um valor válido:");
            numero = Console.ReadLine() ?? "";
            coordenadas = MapearPosicao(int.Parse(numero));
        }

        TabuleiroCompleto[coordenadas.x, coordenadas.y] = jogador ? '=' : 'x';

        TabuleiroCompleto[1, 1] = '1'; 
        TabuleiroCompleto[1, 3] = '1';
        TabuleiroCompleto[3, 1] = '1';
        TabuleiroCompleto[3, 3] = '1';
    }

    private (int x, int y) MapearPosicao(int posicao)
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

    private bool CoordenadasSaoValidas((int x, int y) coordenada)
        => (TabuleiroCompleto[coordenada.x, coordenada.y] == ' ');
}