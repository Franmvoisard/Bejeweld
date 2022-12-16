using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shoelace.Bejeweld.Components
{
    public class WinPopup : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        
        public void Awake()
        {
            restartButton.onClick.AddListener(()=> SceneManager.LoadScene(0));
        }
    }
}