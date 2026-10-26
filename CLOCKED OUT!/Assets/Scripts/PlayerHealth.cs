
using UnityEngine;
using UnityEngine.UI;



public class PlayerHealth : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private float percentage = 0f;
    [SerializeField] private Text playerText;

    void Update()
    {
        if (playerText != null)
        {
            playerText.text = $"Player: {percentage:F0}%";
        }
    }
}

