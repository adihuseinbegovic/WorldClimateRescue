using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Gameplay
{

    /// <summary>
    /// This event is triggered when the player character enters a trigger with a VictoryZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredVictoryZone"></typeparam>
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public int levelCount = 0;
            
        public override void Execute()
        {
            model.player.animator.SetTrigger("victory");
            model.player.controlEnabled = false;
            if(levelCount == 0)
            {
                SceneManager.LoadScene("SecondLevel");
                levelCount++;
            }
            if(levelCount == 1)
            {
                SceneManager.LoadScene("ThirdLevel");
                levelCount++;
            }
            if (levelCount == 2)
            {
                SceneManager.LoadScene("GameCompletedScreen");
                levelCount = 0;
            }

        }
    }
}