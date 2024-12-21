using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tabs.Items
{
    public class WeatherTabItem : MonoBehaviour
    {
        [SerializeField] private Image weatherIcon;
        [SerializeField] private TMP_Text weatherDate;
        [SerializeField] private TMP_Text temperature;

        public CanvasGroup CanvasGroup { get; private set; }

        public void Init(string weatherDate, string temperature)
        {
            this.weatherDate.text = weatherDate;
            this.temperature.text = temperature;
            
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetSprite(Texture2D texture)
        {
            weatherIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); 
        }
    }
}
