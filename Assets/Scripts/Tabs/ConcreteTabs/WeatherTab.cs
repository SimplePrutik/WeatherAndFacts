using System;
using System.Collections.Generic;
using JSONProperties.Weather;
using Tabs.Items;
using UniRx;
using UnityEngine;
using Zenject;

namespace Tabs.ConcreteTabs
{
    public class WeatherTab : BaseTab
    {
        [SerializeField] private WeatherTabItem itemPrefab;

        private List<WeatherTabItem> items = new List<WeatherTabItem>();
        private float weatherSpriteCounter;
        private IDisposable counterDisposable;
        private IDisposable loopRequestDisposable;

        private const string WEATHER_URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public override void Open()
        {
            base.Open();
            webRequestService.SendStringRequest(WEATHER_URL, HandleData);

            loopRequestDisposable = Observable
                .Interval(TimeSpan.FromSeconds(5f))
                .Subscribe(_ =>
                {
                    webRequestService.ClearQueue();
                    webRequestService.SendStringRequest(WEATHER_URL, HandleData);
                });
        }

        public override void Close()
        {
            base.Close();
            items.ForEach(item =>
            {
                Destroy(item.gameObject);
            });
            items.Clear();
            loopRequestDisposable?.Dispose();
        }

        private void HandleData(string data)
        {
            var weatherResponse = JsonUtility.FromJson<WeatherResponse>(data);
            
            weatherSpriteCounter = 0;
            items.ForEach(item =>
            {
                Destroy(item.gameObject);
            });
            items.Clear();
            tabService.OnShowLoader.Execute();
            

            counterDisposable = Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    if (weatherSpriteCounter >= weatherResponse.properties.periods.Length)
                    {
                        tabService.OnHideLoader.Execute();
                        items.ForEach(item => item.CanvasGroup.alpha = 1f);
                        counterDisposable.Dispose();
                    }
                });
            
            foreach (var period in weatherResponse.properties.periods)
            {
                var item = container.InstantiatePrefabForComponent<WeatherTabItem>(itemPrefab, transform);
                item.Init(period.name, period.temperature + period.temperatureUnit);
                webRequestService.SendTextureRequest(period.icon,
                    texture =>
                    {
                        items.Add(item);
                        item.SetSprite(texture);
                        ++weatherSpriteCounter;
                    });
            }
        }
    }
}
