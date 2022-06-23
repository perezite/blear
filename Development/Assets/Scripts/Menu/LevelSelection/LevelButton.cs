namespace Menu.LevelSelection
{
    using System;
    using System.Collections;
    using UnityEngine;

    // Controller for level buttons in the level selection menu
    public class LevelButton : MenuElement
    {
       [Tooltip("Level index represented by the button. The index is counted from the last menu level")]
        public int RelativeTargetLevelIndex;

        // has the level with the given target index already been visited
        public bool WasLevelVisitedByPlayer()
        {
            int lastVisitedGameLevel = GameManager.GetInstance().GetSavegame().Load().LastVisitedGameLevel;
            return Levels.GetAbsoluteLevelIndex(RelativeTargetLevelIndex) <= lastVisitedGameLevel;
        }

        // enable based on current player's progress
        public override void SetInteractable(bool becomeInteractable)
        {
            bool interactable = false;

            if (becomeInteractable == true && WasLevelVisitedByPlayer())
            {
                interactable = true;
            }

            base.SetInteractable(interactable);
        }

        protected override IEnumerator OnClickHandler()
        {
            StartCoroutine(GameManager.GetInstance().GoToLevelWithLoadingIndicator(Levels.GetAbsoluteLevelIndex(RelativeTargetLevelIndex)));
            yield return null;
        }

        private void Awake()
        {
            SetInteractable(true);
        }
    }
}