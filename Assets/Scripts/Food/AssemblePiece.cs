using System.Collections.Generic;
using UnityEngine;

public class AssemblePiece : MonoBehaviour
{
    public int id;
    public int[] RequiredNeighbors;
    [HideInInspector] public Dictionary<int, AssemblePiece> touching = new();

    [HideInInspector] public BurgerAssembler burgerAssembler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<AssemblePiece>(out AssemblePiece piece))
        {
            if (!touching.ContainsKey(piece.id))
            {
                touching.Add(piece.id, piece);
                CheckCompletion();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<AssemblePiece>(out AssemblePiece piece))
        {
            if (touching.ContainsKey(piece.id))
            {
                touching.Remove(piece.id);
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
            if (touching.Count != RequiredNeighbors.Length) { return; }

            foreach (int id in piece.RequiredNeighbors)
            {
                if (!piece.touching.ContainsKey(id)) { return; }
                if (!assemblePieces.Contains(piece.touching[id])) 
                { 
                    assemblePieces.Add(piece.touching[id]);
                    stack.Push(piece);
                }
            }
        }

        burgerAssembler.Assemble();
    }
}