using Shoelace.Bejeweld.Views;
using UnityEngine;

namespace Shoelace.Bejeweld.Components
{
    public class SoundFeedback : MonoBehaviour
    {
        [SerializeField] private AudioClip matchSound;
        [SerializeField] private AudioClip fillSound;
        private int _chainCount = 0;

        private void Awake()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            GridView.OnMatchesCleared += PlayMatchSound;
            GridView.OnChainFinished += ResetChainCount;
            GridView.OnTilesDropped += PlayFillSound;
            GridView.OnRefillComplete += PlayFillSound;
        }

        private void PlayMatchSound(int _)
        {
            AudioController.PlaySound(matchSound, 1 + _chainCount * 0.06f);
            _chainCount++;
        }

        private void PlayFillSound()
        {
            AudioController.PlaySound(fillSound);
        }

        private void UnsubscribeToEvents()
        {
            GridView.OnMatchesCleared -= PlayMatchSound;
            GridView.OnChainFinished -= ResetChainCount;
            GridView.OnTilesDropped -= PlayFillSound;
            GridView.OnRefillComplete -= PlayFillSound;
        }

        private void ResetChainCount()
        {
            _chainCount = 0;
        }

        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }
    }
}