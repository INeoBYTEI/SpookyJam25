using System.Collections;
using UnityEngine;

public class BurgerAssembler : FoodStationWorkArea
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float assembleTime;
    [SerializeField] GameObject burgerPiecesPrefab;
    [SerializeField] Transform[] assembleSpots;
    AssemblePiece[] assemblePieces;

    float timeStartedAssemble;

    public override void Activate(FoodType foodType)
    {
        base.Activate(foodType);

        GameObject burgerPieces = Instantiate(burgerPiecesPrefab, transform);
        assemblePieces = burgerPieces.GetComponentsInChildren<AssemblePiece>();

        foreach (AssemblePiece assemblePiece in assemblePieces)
        {
            assemblePiece.burgerAssembler = this;
        }
    }

    public void Assemble()
    {
        timeStartedAssemble = Time.time;
        for (int i = 0; i < assembleSpots.Length && i < assembleSpots.Length; i++)
        {
            StartCoroutine(LerpToSpot(assemblePieces[i].transform.parent, assembleSpots[i], assemblePieces[i].transform.position, assemblePieces[i].transform.rotation));
            assemblePieces[i].transform.parent.GetComponent<Collider2D>().enabled = false;
            Rigidbody2D rigidbody = assemblePieces[i].transform.parent.GetComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;

            assemblePieces[i].enabled = false;
        }

        Invoke(nameof(Finished), assembleTime + 0.1f);
    }

    private void Finished()
    {
        FoodReferenceTable.Instance.SpawnFood(foodType, transform.position);
        Deactivate();

        foreach (AssemblePiece assemblePiece in assemblePieces)
        {
            Destroy(assemblePiece.transform.parent.gameObject);
        }
    }

    IEnumerator LerpToSpot(Transform mover, Transform target, Vector3 startPos, Quaternion startRotation)
    {
        float normalizedTime = (Time.time - timeStartedAssemble) / assembleTime;
        float value;
        while (normalizedTime < 1)
        {
            if (mover == null || target == null) { break; }
            normalizedTime = (Time.time - timeStartedAssemble) / assembleTime;
            value = curve.Evaluate(normalizedTime);
            mover.position = Vector3.Lerp(startPos, target.position, value);
            mover.rotation = Quaternion.Slerp(startRotation, Quaternion.identity, value);
            yield return new WaitForEndOfFrame();
        }

        if (mover != null && target != null)
        {
            mover.position = target.position;
        }
    }
}