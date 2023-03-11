namespace DotsGame;

public class Tabuleiro
{
    

    public char[,] TabuleiroCompleto { get; private set; }
    public Tabuleiro(){
        TabuleiroCompleto = new char[5, 5];
        
    }

}