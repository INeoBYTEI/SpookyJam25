using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssemblePiece : MonoBehaviour
{
    public int id;

    [HideInInspector] public HashSet<AssemblePiece> touching = new();
    [HideInInspector] public BurgerAssembler burgerAssembler;


    private void OnEnable()
    {
        touching.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AssemblePiece assemblePiece = collision.GetComponentInChildren<AssemblePiece>();
        if (assemblePiece)
        {
            if (!touching.Contains(assemblePiece))
            {
                touching.Add(assemblePiece);
                CheckCompletion();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        AssemblePiece assemblePiece = collision.GetComponentInChildren<AssemblePiece>();
        if (assemblePiece)
        {
            if (touching.Contains(assemblePiece))
            {
                touching.Remove(assemblePiece);
                CheckCompletion();
            }
        }
    }

    private void CheckCompletion()
    {
        HashSet<AssemblePiece> assemblePieces = new();
        Stack<AssemblePiece> stack = new();
        stack.Push(this);

        while (stack.Count > 0)
        {
            AssemblePiece piece = stack.Pop();
            if (piece.id == 1 || piece.id == 3)
            {
                if (piece.touching.Count != 1) { return; }
            }
            foreach (var kvp in piece.touching)
            {
                if (!assemblePieces.Contains(kvp))
                {
                    stack.Push(kvp);
                    assemblePieces.Add(kvp);  
                }
            }
        }

        if(assemblePieces.Count != 6) {  return; }
        burgerAssembler.Assemble();
    }
}