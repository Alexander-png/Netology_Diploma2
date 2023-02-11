using Platformer.GameCore;
using UnityEngine;

namespace Platformer.UI.MenuSystem.Items
{
    public class SelectLevelMenuItem : MenuItem
    {
        [SerializeField]
        private string _levelName;

        public override object Data => _levelName;

        protected override void Start()
        {
            base.Start();
            SaveSystem.GetLevelInfo(_levelName);
        }
    }
}