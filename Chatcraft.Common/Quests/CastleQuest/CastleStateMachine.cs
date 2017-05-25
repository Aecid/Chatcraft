using System;
using System.Collections.Generic;
using System.Text;

namespace Chatcraft.Common.Quests.CastleQuest
{
    
    public enum CastleProcessState
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum CastleCommand
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }

    /// <summary>
    /// Конечный автомат для квеста "Заброшенный Замок"
    /// </summary>
    public class CastleStateMachine
    {
        class StateTransition
        {
            readonly CastleProcessState CurrentState;
            readonly CastleCommand Command;

            public StateTransition(CastleProcessState currentState, CastleCommand command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        Dictionary<StateTransition, CastleProcessState> transitions;
        public CastleProcessState CurrentState { get; private set; }

        public CastleStateMachine()
        {
            CurrentState = CastleProcessState.Inactive;
            transitions = new Dictionary<StateTransition, CastleProcessState>
            {
                { new StateTransition(CastleProcessState.Inactive, CastleCommand.Exit), CastleProcessState.Terminated },
                { new StateTransition(CastleProcessState.Inactive, CastleCommand.Begin), CastleProcessState.Active },
                { new StateTransition(CastleProcessState.Active, CastleCommand.End), CastleProcessState.Inactive },
                { new StateTransition(CastleProcessState.Active, CastleCommand.Pause), CastleProcessState.Paused },
                { new StateTransition(CastleProcessState.Paused, CastleCommand.End), CastleProcessState.Inactive },
                { new StateTransition(CastleProcessState.Paused, CastleCommand.Resume), CastleProcessState.Active }
            };
        }

        public CastleProcessState GetNext(CastleCommand command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            CastleProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public CastleProcessState MoveNext(CastleCommand command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }
    }


  
}
