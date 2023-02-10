using Platformer.CharacterSystem.Base;
using Platformer.UI.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
	public class InWorldHealthBar : MonoBehaviour
	{
        private IDamagable _damagableOwner;
        private float _currentHealth;
        private Image _healthBarForeground;
        private TMP_Text _text;

        private void Start()
        {
            _damagableOwner = GetComponentInParent<IDamagable>();
            _healthBarForeground = GetComponentInChildren<InWorldHeathBarForeground>().GetComponent<Image>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            if (_currentHealth != _damagableOwner.CurrentHealth)
            {
                _currentHealth = _damagableOwner.CurrentHealth;
                _healthBarForeground.fillAmount = _currentHealth / _damagableOwner.MaxHealth;
                _text.text = $"{_healthBarForeground.fillAmount * 100}%";
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }
}