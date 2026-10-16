using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CardBattlerCourse.Enemies
{
    public class EnemyIntentUI : MonoBehaviour
    {
        [SerializeField] private Image intentIcon;
        [SerializeField] private TextMeshProUGUI valueText;

        [SerializeField] private Sprite attackIcon;
        [SerializeField] private Sprite buffIcon;
        [SerializeField] private Sprite guardIcon;

        private Dictionary<IntentType, Sprite> enemyIntentSpritesDict;

        private void Awake()
        {
            enemyIntentSpritesDict = new()
            {
                [IntentType.Attack] = attackIcon,
                [IntentType.Buff] = buffIcon,
                [IntentType.Guard] = guardIcon,
            };
        }

        public void SetIntent(EnemyIntent intent)
        {
            gameObject.SetActive(true);
            valueText.text = intent.Value.ToString();

            if (enemyIntentSpritesDict.TryGetValue(intent.Type, out Sprite newIntentSprite))
                intentIcon.sprite = newIntentSprite;
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}