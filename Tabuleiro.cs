namespace DotsGame;

public class Tabuleiro
{
    // Será removido nas versões futuras

    public char[,] TabuleiroCompleto { get; private set; }
    public Tabuleiro(){
        TabuleiroCompleto = new char[5, 5];
    }
}