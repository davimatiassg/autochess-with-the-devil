using Godot;
using System.Collections.Generic;

//Rascunho do contexto da BT
//tem as infos do estado do jogo e contexto pra ia tomar decisão, é limpo e preenchido todo turno, funciona meio que como estado e ação
public partial class AIContext : RefCounted
{
    //Definindo aqui o DevilHand, as cartas na mão, e os espaços que da pra jogar
    public DevilHand Hand { get; private set; }
    public List<CardEffect> CardsInHand { get; private set; }
    public List<TabletopTile> PlayableTiles { get; private set; }


    //Pra ia inferir o contexto
    public AIContext(DevilHand hand, List<CardEffect> cards, List<TabletopTile> tiles)
    {
        this.Hand = hand;
        this.CardsInHand = cards;
        this.PlayableTiles = tiles;
    }

    //faz a att do contexto com as info recentes
    //Chama no inicio do turno do djabo
    public void Update(List<CardEffect> cards, List<TabletopTile> tiles)
    {
        this.CardsInHand = cards;
        this.PlayableTiles = tiles;
    }
}