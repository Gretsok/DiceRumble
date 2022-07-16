using System;
using System.Collections.Generic;
using UnityEngine;

namespace DR.Gameplay.Dices.Manager
{
    public class DicesManager : MonoBehaviour
    {
        private List<Dice> m_firstTeamDices = new List<Dice>();
        private List<Dice> m_secondTeamDices = new List<Dice>();
        public List<Dice> FirstTeamDices => m_firstTeamDices;
        public List<Dice> SecondTeamDices => m_secondTeamDices;

        public Action OnTeamEliminated = null;

        public void SetUpDices(Teams.TeamsDataConveyor a_teamsDataConveyor, Level.Grid.Grid a_grid)
        {
            for (int i = 0; i < a_teamsDataConveyor.FirstTeamDicesData.Count; ++i)
            {
                var newDice = Instantiate(a_teamsDataConveyor.FirstTeamDicesData[i].DicePrefab, a_grid.transform);
                newDice.InflateArmsColors((MOtter.MOtt.GM as DRGameManager).GlobalGameData.FirstTeamData.TeamColor);
                var spawnTile = a_grid.TryToGetTile(a_grid.FirstTeamSpawnPositions[i]);
                spawnTile.TryToSetCurrentDice(newDice);
                newDice.GetComponent<DiceMovementController>().GamePosition = a_grid.FirstTeamSpawnPositions[i];
                newDice.transform.position = spawnTile.transform.position;
                newDice.transform.rotation = spawnTile.transform.rotation;
                var extraBrain = newDice.GetComponent<DiceMovementController>().ExtrasRotationBrain;
                var grid = MOtter.MOtt.GM.GetCurrentMainStateMachine<Level.Flow.LevelGameMode>().Grid;
                extraBrain.SetUp(grid);
                newDice.Init(a_teamsDataConveyor.FirstTeamDicesData[i].DiceHealth, 0);
                m_firstTeamDices.Add(newDice);
            }

            for (int i = 0; i < a_teamsDataConveyor.SecondTeamDicesData.Count; ++i)
            {
                var newDice = Instantiate(a_teamsDataConveyor.SecondTeamDicesData[i].DicePrefab, a_grid.transform);
                newDice.InflateArmsColors((MOtter.MOtt.GM as DRGameManager).GlobalGameData.SecondTeamData.TeamColor);
                var spawnTile = a_grid.TryToGetTile(a_grid.SecondteamSpawnPositions[i]);
                spawnTile.TryToSetCurrentDice(newDice);
                newDice.GetComponent<DiceMovementController>().GamePosition = a_grid.SecondteamSpawnPositions[i];
                newDice.transform.position = spawnTile.transform.position;
                newDice.transform.rotation = spawnTile.transform.rotation;
                var extraBrain = newDice.GetComponent<DiceMovementController>().ExtrasRotationBrain;
                var grid = MOtter.MOtt.GM.GetCurrentMainStateMachine<Level.Flow.LevelGameMode>().Grid;
                extraBrain.SetUp(grid);
                extraBrain.transform.forward = -grid.transform.forward;
                newDice.Init(a_teamsDataConveyor.SecondTeamDicesData[i].DiceHealth, 1);
                m_secondTeamDices.Add(newDice);
            }
        }

        public void HandleDiceDeath(Dice a_deadDice)
        {
            if(m_firstTeamDices.Contains(a_deadDice))
            {
                m_firstTeamDices.Remove(a_deadDice);
                if(m_firstTeamDices.Count <= 0)
                {
                    OnTeamEliminated?.Invoke();
                }
            }
            if(m_secondTeamDices.Contains(a_deadDice))
            {
                m_secondTeamDices.Remove(a_deadDice);
                if (m_secondTeamDices.Count <= 0)
                {
                    OnTeamEliminated?.Invoke();
                }
            }
        }
    }
}