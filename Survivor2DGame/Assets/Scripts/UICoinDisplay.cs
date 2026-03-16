using TMPro;
using UnityEngine;

// Script pour afficher les pièces du joueur dans l'interface
public class UICoinDisplay : MonoBehaviour
{
    // Texte UI qui va afficher les pièces
    TextMeshProUGUI displayTarget;

    // Référence au joueur qui collecte les pièces
    public PlayerCollector collector;

    void Start()
    {
        // Trouve le texte dans l'objet
        displayTarget = GetComponentInChildren<TextMeshProUGUI>();

        // Met à jour l'affichage
        UpdateDisplay();

        // Quand le joueur ramasse une pièce, on met à jour le texte
        if (collector != null)
            collector.onCoinCollected += UpdateDisplay;
    }

    // Fonction appelée quand on reset le script dans Unity
    private void Reset()
    {
        // Cherche automatiquement un PlayerCollector
        collector = FindObjectOfType<PlayerCollector>();
    }

    // Fonction pour mettre à jour le nombre de pièces affichées
    public void UpdateDisplay()
    {
        // Si un joueur est assigné
        if (collector != null)
        {
            // On affiche ses pièces actuelles
            displayTarget.text = Mathf.RoundToInt(collector.GetCoins()).ToString();
        }
        else
        {
            // Sinon on affiche les pièces sauvegardées
            float coins = SaveManager.LastLoadedGameData.coins;
            displayTarget.text = Mathf.RoundToInt(coins).ToString();
        }
    }
}