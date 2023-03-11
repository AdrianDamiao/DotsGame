namespace DotsGame
{
    public class No
    {
        public int ValorMinMax { get; set; }
        public List<No> Filhos { get; set; }
        private const int TamanhoDoTabuleiro = 5; 
        private const char EspacoVazio = ' ';
        private const char CaracterDePonto = '*';
        private const char Jogador1 = '1'; 
        private const char Jogador2 = '2'; 
        public char[,] Tabuleiro { get; private set; }

        public No()
        {
            Filhos = new List<No>();
            Tabuleiro = new char[5, 5];
            ResetaTabuleiro();
        }

        public void CopiaMatriz(No noPai)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tabuleiro[i, j] = noPai.Tabuleiro[i, j];
                }
            }
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
            for(int i = 0; i < TamanhoDoTabuleiro; i++)
                if(i % 2 == 0)
                    for(int j = 0; j < TamanhoDoTabuleiro; j++)
                        if(j % 2 == 0)
                            Tabuleiro[i, j] = '*';
        }

        private void LimparJogadas()
        {
            for(int i = 0; i < TamanhoDoTabuleiro; i++)
            {
                if(i % 2 == 0){
                    for(int j = 0; j < TamanhoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = ' ';
                } else {
                    for(int k = 0; k < TamanhoDoTabuleiro; k++)
                        if(k % 2 == 0)
                            Tabuleiro[i, k] = ' ';
                }
            }
        }

        private void LimparPontuacao()
        {
            for(int i = 0; i < TamanhoDoTabuleiro; i++)
                if(i % 2 == 1)
                    for(int j = 0; j < TamanhoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = ' ';
        }

        public void ExibirTabuleiro()
        {
            Console.WriteLine("Exibindo tabuleiro...\n");

            for (int i = 0; i < TamanhoDoTabuleiro; i++)
            {
                for (int j = 0; j < TamanhoDoTabuleiro; j++)
                {
                    Console.Write(" " + Tabuleiro[i, j] + " ");
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
            for (int i = 0; i < TamanhoDoTabuleiro; i++)
                for (int j = 0; j < TamanhoDoTabuleiro; j++)
                    Tabuleiro[i, j] = ' ';
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
            for(int i = 0; i < TamanhoDoTabuleiro; i++)
            {
                if(i % 2 == 0){
                    for(int j = 0; j < TamanhoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = '-';
                } else {
                    for(int k = 0; k < TamanhoDoTabuleiro; k++)
                        if(k % 2 == 0)
                            Tabuleiro[i, k] = '|';
                }
            }
        }

        private void PreencheExemploDePontuacao()
        {
            for(int i = 0; i < TamanhoDoTabuleiro; i++)
                if(i % 2 == 1)
                    for(int j = 0; j < TamanhoDoTabuleiro; j++)
                        if(j % 2 == 1)
                            Tabuleiro[i, j] = '2';
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
                if(Tabuleiro[coordenada.linha, coordenada.coluna] == 'x')
                    Console.WriteLine("Outro jogador já fez essa jogada!");

                Console.WriteLine("Digite um valor válido:");
                numero = Console.ReadLine() ?? "";
                coordenada = MapearPosicao(int.Parse(numero));
            }

            Tabuleiro[coordenada.linha, coordenada.coluna] = 'x';

            VerificarPontuacao(coordenada, jogador);
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

        public bool JogadasFinalizadas()
            => (Tabuleiro[1, 1] != ' ' 
                && Tabuleiro[1, 3] != ' ' 
                && Tabuleiro[3, 1] != ' ' 
                && Tabuleiro[3, 3] != ' ');

        private bool CoordenadasSaoValidas((int linha, int coluna) coordenada)
            => (Tabuleiro[coordenada.linha, coordenada.coluna] == ' ');

        private void VerificarPontuacao((int linha, int coluna) coordenada, bool jogador)
        {
            bool jogadorPontuou = false;

            // Verifica pra esquerda, se puder
            if(coordenada.coluna - 2 >= 0 && (coordenada.linha - 1 >= 0 && coordenada.linha + 1 < TamanhoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoAEsquerda(coordenada.linha, coordenada.coluna, jogador);

            // Verifica pra direita, se puder
            if(coordenada.coluna + 2 < TamanhoDoTabuleiro && (coordenada.linha - 1 >= 0 && coordenada.linha + 1 < TamanhoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoADireita(coordenada.linha, coordenada.coluna, jogador);

            // //Verifica pra cima, se puder
            if(coordenada.linha - 2 >= 0 && (coordenada.coluna - 1 >= 0 && coordenada.coluna + 1 < TamanhoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoAcima(coordenada.linha, coordenada.coluna, jogador);

            //Verifica pra baixo, se puder
            if(coordenada.linha + 2 < TamanhoDoTabuleiro && (coordenada.coluna - 1 >= 0 && coordenada.coluna + 1 < TamanhoDoTabuleiro))
                jogadorPontuou = VerificaQuadradoAbaixo(coordenada.linha, coordenada.coluna, jogador);

            if(jogadorPontuou)
                LiberarOutraJogada(jogador);
        }

        private bool VerificaQuadradoAEsquerda(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha, coluna - 2] != EspacoVazio
                && Tabuleiro[linha + 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha, coluna - 1] != CaracterDePonto)
            {
                Tabuleiro[linha, coluna - 1] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private bool VerificaQuadradoADireita(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha, coluna + 2] != EspacoVazio
                && Tabuleiro[linha + 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha, coluna + 1] != CaracterDePonto)
            {
                Tabuleiro[linha, coluna + 1] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private bool VerificaQuadradoAcima(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha - 2, coluna] != EspacoVazio
                && Tabuleiro[linha - 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha - 1, coluna] != CaracterDePonto)
            {
                Tabuleiro[linha - 1, coluna] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }
        private bool VerificaQuadradoAbaixo(int linha, int coluna, bool jogador)
        {
            if(Tabuleiro[linha, coluna] != EspacoVazio
                && Tabuleiro[linha + 2, coluna] != EspacoVazio
                && Tabuleiro[linha + 1, coluna + 1] != EspacoVazio
                && Tabuleiro[linha + 1, coluna - 1] != EspacoVazio
                && Tabuleiro[linha + 1, coluna] != CaracterDePonto)
            {
                Tabuleiro[linha + 1, coluna] = SimboloDoJogador(jogador);
                return true;
            }
            return false;
        }

        private void LiberarOutraJogada(bool jogador)
        {
            ExibirTabuleiro();
            System.Console.WriteLine("Parabéns, você joga de novo!");
            MarcarJogada(jogador);
        }

        private char SimboloDoJogador(bool jogador)
            => jogador ? Jogador1 : Jogador2;
    }
}