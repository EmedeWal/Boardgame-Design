using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Health _health;

    private void Start()
    {
        _health.Death += GameManager_Death;
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale);

        foreach (Collider hit in hits )
        {
            HandleEnemyEnter(hit);
        }
    }

    private void HandleEnemyEnter(Collider other)
    {
        Debug.Log("Trigger entered");

        int cost = other.GetComponent<Unit>().GetCost();
        other.GetComponent<Health>().TakeDamage(1000);

        _health.TakeDamage(cost);
    }

    private void GameManager_Death(GameObject gameObject)
    {
        SceneManager.LoadScene("End Screen");
    }
}
