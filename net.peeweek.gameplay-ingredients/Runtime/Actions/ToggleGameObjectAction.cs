using NaughtyAttributes;
using UnityEngine;

namespace GameplayIngredients.Actions
{
    public class ToggleGameObjectAction : ActionBase
    {
        [ReorderableList]
        public GameObjectToggle[] Targets;

        public override void Execute()
        {
            foreach(var target in Targets)
            {
                if (target.GameObject == null)
                {
                    Debug.Log("Target is null, ignoring");
                }
                else
                {
                    switch(target.State)
                    {
                        case GameObjectToggle.GameObjectToggleState.Disable:
                            target.GameObject.SetActive(false);
                            break;
                        case GameObjectToggle.GameObjectToggleState.Enable:
                            target.GameObject.SetActive(true);
                            break;
                        case GameObjectToggle.GameObjectToggleState.Toggle:
                            target.GameObject.SetActive(!target.GameObject.activeSelf);
                            break;
                    }
                }
            }
        }

        [System.Serializable]
        public struct GameObjectToggle
        {
            [System.Serializable]
            public enum GameObjectToggleState
            {
                Disable = 0,
                Enable = 1,
                Toggle = 2
            }

            public GameObject GameObject;
            public GameObjectToggleState State;
        }
    }
}
