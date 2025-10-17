using UnityEngine;

public class DragSpawner : Clickable
{
    [SerializeField] GameObject spawnPrefab;

    public override void OnClick()
    {
        GameObject spawned = Instantiate(spawnPrefab, transform.position, Quaternion.identity);
        if (spawned != null)
        {
            if (spawned.TryGetComponent<Clickable>(out Clickable clickable))
            {
                PostSpawn(clickable);
            }
        }
    }

    private void PostSpawn(Clickable clickable)
    {
        Clicker.Instance.AddClicked(clickable);
    }
}
