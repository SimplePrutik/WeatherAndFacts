using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tabs.Items
{
    public class FactsTabItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text breedId;
        [SerializeField] private TMP_Text breedName;
        [SerializeField] private Loader loader;
        [SerializeField] private Button button;

        public Button Button => button;
        public Loader Loader => loader;

        public CanvasGroup CanvasGroup { get; private set; }

        public void Init(string breedId, string breedName)
        {
            this.breedId.text = breedId;
            this.breedName.text = breedName;
            loader.SetEnable(false);
            CanvasGroup = GetComponent<CanvasGroup>();
        }
    }
}